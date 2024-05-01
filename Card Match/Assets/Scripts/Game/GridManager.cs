using UnityEngine;

namespace Game.Grid
{
    public class GridManager
    {
        private CameraManager currentCamera;

        private Sprite gridImage;

        public float SpriteWidth => gridImage.rect.width / gridImage.pixelsPerUnit;

        public float SpriteHeight => gridImage.rect.height / gridImage.pixelsPerUnit;
        private float leftSide => currentCamera.LeftX + (SpriteWidth / 2f);

        internal float downSide => currentCamera.DownY + (SpriteHeight / 2f);

        private bool drawByWidth;
        private int verticalLength;
        private int horizontalLength;

        private float offset;

        public void Init()
        {
            currentCamera = Camera.main.GetComponent<CameraManager>();
        }

        public void SetSprite(Sprite sprite)
        {
            gridImage = sprite;
        }

        public void SetLength(int horizontal, int vertical)
        {
            horizontalLength = horizontal;
            verticalLength = vertical;
        }

        public void SetOffset(float offset)
        {
            this.offset = offset;
        }

        public Vector3 GetPosition(int i, int j)
        {
            float offset = this.offset;

            float downstarter = -1 * ((verticalLength - 1) * (SpriteHeight + offset)) / 2f;
            float leftstarter = -1 * ((horizontalLength - 1) * (SpriteWidth + offset)) / 2f;
            float x, y;

            if (drawByWidth)
            {
                x = leftSide + (i * SpriteWidth) + (i + 1) * offset;
                y = downstarter + (j * (SpriteHeight + offset));
            }
            else
            {
                x = leftstarter + i * (SpriteWidth + offset);
                y = downSide + (j * SpriteHeight) + (j + 1) * offset;
            }

            return new Vector3(x, y, 0);
        }

        public void SetCamera()
        {
            if (currentCamera == null || currentCamera.Camera == null)
                return;

            currentCamera.Camera.ResetAspect();
            float totalcoveredwidth, totalcoveredheight;

            // Considering by width wise
            if ((currentCamera.Camera.aspect * (1 + (verticalLength - 1)) / horizontalLength) <= 1)
            {
                totalcoveredwidth = SpriteWidth * horizontalLength; // 1.28*8 = 10.24

                totalcoveredwidth += (horizontalLength + 1) * offset;

                currentCamera.SetOrthoGraphicSize(totalcoveredwidth / (currentCamera.Camera.aspect * 2));
                drawByWidth = true;
            }

            //Considering by height wise
            else
            {
                totalcoveredheight = SpriteHeight * (1 + (verticalLength - 1)); // 1.28*12 = 15.36

                totalcoveredheight += (verticalLength + 1) * offset;

                currentCamera.SetOrthoGraphicSize(totalcoveredheight / 2);
                drawByWidth = false;
            }
        }
    }
}