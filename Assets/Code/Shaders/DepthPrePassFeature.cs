using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class DepthPrePassFeature : ScriptableRendererFeature
{
    class DepthPrePass : ScriptableRenderPass
    {
        private ShaderTagId shaderTagId = new ShaderTagId("DepthOnly");
        private FilteringSettings filteringSettings;

        public DepthPrePass(RenderQueueRange renderQueueRange, LayerMask layerMask)
        {
            filteringSettings = new FilteringSettings(renderQueueRange, layerMask);
        }

        public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
        {
            CommandBuffer cmd = CommandBufferPool.Get("DepthPrePass");
            using (new ProfilingScope(cmd, new ProfilingSampler("DepthPrePass")))
            {
                context.ExecuteCommandBuffer(cmd);
                cmd.Clear();

                var drawSettings = CreateDrawingSettings(shaderTagId, ref renderingData, SortingCriteria.CommonOpaque);
                context.DrawRenderers(renderingData.cullResults, ref drawSettings, ref filteringSettings);
            }
            context.ExecuteCommandBuffer(cmd);
            CommandBufferPool.Release(cmd);
        }
    }

    [SerializeField] private RenderPassEvent renderPassEvent = RenderPassEvent.AfterRenderingOpaques;
    [SerializeField] private LayerMask layerMask = ~0;

    private DepthPrePass depthPrePass;

    public override void Create()
    {
        depthPrePass = new DepthPrePass(RenderQueueRange.transparent, layerMask)
        {
            renderPassEvent = renderPassEvent
        };
    }

    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        renderer.EnqueuePass(depthPrePass);
    }
}
