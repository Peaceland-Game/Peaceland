namespace FractalShader {
    using UnityEngine;
    using UnityEngine.UI;

    public class Global : MonoBehaviour {

        //Properties.
        public Text FPSText;
        public Transform mainMenu;
        public Transform framesPerSecondAndBackButton;
        public Transform fractals, fractalUserInterfaces;
        public Transform gameObjectsParent;
        public Transform gameObjectsQuad, gameObjectsSphere, gameObjectsCube, gameObjectsSprite;

        //Variables.
        int frameCount = 0;
        float frameTime = 0;
        float everyTenthOfASecond = 0;

        //Update.
        void Update() {

            //Update the frames per second.
            frameCount++;
            frameTime += Time.deltaTime;
            if (frameTime > 1.0f) {
                FPSText.text = $"{(int) (frameCount / frameTime)} FPS";
                frameCount = 0;
                frameTime -= 1.0f;
            }

            //Rotate the game objects fractals.
            float aspectRatio = (float) Screen.width / Screen.height;
            float scale = aspectRatio / 1.777777777778f;
            gameObjectsParent.localScale = new Vector3(scale, scale, scale);
            gameObjectsQuad.Rotate(0, Time.deltaTime * 48f, 0);
            gameObjectsSphere.Rotate(Time.deltaTime * 28f, Time.deltaTime * 41f, Time.deltaTime * 55f);
            gameObjectsCube.Rotate(Time.deltaTime * 28f, Time.deltaTime * 41f, Time.deltaTime * 55f);
            gameObjectsSprite.Rotate(0, 0, Time.deltaTime * 48f);

            //Animate the "static noise" fractal.
            everyTenthOfASecond += Time.deltaTime;
            if (everyTenthOfASecond >= 0.1f) {
                fractals.Find("Static Noise Fractal Quad").GetComponent<MeshRenderer>().material.SetFloat("juliaConstantImaginary", Random.Range(0f, 0.1f));
                everyTenthOfASecond -= 0.1f;
            }
        }

        //Show a fractal.
        public void showFractal(string name) {
            mainMenu.gameObject.SetActive(false);
            framesPerSecondAndBackButton.gameObject.SetActive(true);
            for (int i = 0; i < fractals.childCount; i++)
                fractals.GetChild(i).gameObject.SetActive(fractals.GetChild(i).name.IndexOf(name) == 0);
            for (int i = 0; i < fractalUserInterfaces.childCount; i++)
                fractalUserInterfaces.GetChild(i).gameObject.SetActive(fractalUserInterfaces.GetChild(i).name.IndexOf(name) == 0);
        }

        //Return to the main menu.
        public void backToMainMenu() {
            mainMenu.gameObject.SetActive(true);
            framesPerSecondAndBackButton.gameObject.SetActive(false);
            for (int i = 0; i < fractals.childCount; i++)
                fractals.GetChild(i).gameObject.SetActive(false);
            for (int i = 0; i < fractalUserInterfaces.childCount; i++)
                fractalUserInterfaces.GetChild(i).gameObject.SetActive(false);
        }

        //Various slider/checkbox changes.
        public void colouredBackgroundSliderValueChanged(float value) {
            fractals.Find("Coloured Background Fractal Quad").GetComponent<MeshRenderer>().material.SetFloat("backgroundColourAmount", value);
            GameObject.Find("Coloured Background White Colour Slider Text").GetComponent<Text>().text = $"{(int) (value * 100)}% white";
        }
        public void toggleSmooth(bool smooth) {
            fractals.Find("Smoothing Fractal Quad").GetComponent<MeshRenderer>().material.SetInt("smoothing", smooth ? 1 : 0);
        }
        public void iterationsPerPixelSliderValueChanges(float value) {
            fractals.Find("Iterations Fractal Quad").GetComponent<MeshRenderer>().material.SetInt("iterations", (int) value);
            GameObject.Find("Iterations Slider Text").GetComponent<Text>().text = $"{(int) value} iterations per pixel";
        }
        public void sunsetSliderValueChanged(float value) {
            MeshRenderer meshRenderer = fractals.Find("Sunset Fractal Quad").GetComponent<MeshRenderer>();
            meshRenderer.material.SetInt("iterations", (int) (value * 9.0f) + 6);
            meshRenderer.material.SetFloat("centreY", (value * 0.68f) + 1.62f);
            meshRenderer.material.SetFloat("redMultiplier", (value * -0.5f) + 1.0f);
            meshRenderer.material.SetFloat("greenMultiplier", (value * -1.3f) + 1.8f);
            meshRenderer.material.SetFloat("blueMultiplier", (value * -1.0f) + 3.0f);
        }
        public void flowerSliderValueChanged(float value) {
            MeshRenderer meshRenderer = fractals.Find("Flower Fractal Quad").GetComponent<MeshRenderer>();
            float multiplierValue = Mathf.Pow(10, value - 6);
            meshRenderer.material.SetFloat("redMultiplier", multiplierValue);
            meshRenderer.material.SetFloat("greenMultiplier", multiplierValue);
            meshRenderer.material.SetFloat("blueMultiplier", multiplierValue);
            GameObject.Find("Flower Slider Text").GetComponent<Text>().text = $"Multiplier: {multiplierValue:F5}";
        }
        public void starFieldBlackSliderValueChanges(float value) {
            MeshRenderer meshRenderer = fractals.Find("Star Field Fractal Quad").GetComponent<MeshRenderer>();
            meshRenderer.material.SetFloat("backgroundColourAmount", value);
            GameObject.Find("Star Field Black Slider Text").GetComponent<Text>().text = $"{value * 100:F2}% black";
        }
    }
}