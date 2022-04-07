using UnityEngine;

namespace Utils
{
    public static class MergeUtils
    {
        public static float CellHeight = 151f / 256f;
        public static Vector3 HalfCellUp = new Vector3(0, CellHeight / 2f);
        
        public static Vector2 ToV2(this Vector3Int vector3Int) => new Vector2(vector3Int.x, vector3Int.y);
        public static Vector3Int ToTileXY(this Vector2 worldPos, Grid grid) =>
            grid.WorldToCell(worldPos);

        public static (int, int) RoundMousePos(this Camera camera)
        {
            var pos = camera.ScreenToWorldPoint(Input.mousePosition);
            var posX = Mathf.RoundToInt(pos.x);
            var posY = Mathf.RoundToInt(pos.y);
            return (posX, posY);
        }
         public static (int, int) Round(this Vector2 pos)
        {
            var posX = Mathf.RoundToInt(pos.x);
            var posY = Mathf.RoundToInt(pos.y);
            return (posX, posY);
        }
        public static (int, int) Round(this Vector3 pos)
        {
            var posX = Mathf.RoundToInt(pos.x);
            var posY = Mathf.RoundToInt(pos.y);
            return (posX, posY);
        }
        
        public static Vector2 WorldToTileCenter(this Vector2 worldPos, Grid grid) =>
            grid.CellToWorld(grid.WorldToCell(worldPos) ) + HalfCellUp;

        public static Vector2 TileCenter(this Vector3Int tileXY, Grid grid) =>
            grid.CellToWorld(tileXY) + HalfCellUp;

        public static Vector3 ScreenToWorld(this Vector3 pos, Camera camera)
        {
            pos.z = camera.nearClipPlane;
            return camera.ScreenToWorldPoint(pos);
        }
        public static Vector2 ScreenToWorld(this Vector2 pos, Camera camera)
        {
            // pos.z = camera.nearClipPlane;
            return camera.ScreenToWorldPoint(pos);
        }

        public static Vector2 WorldToScreen(this Vector3 pos, Camera camera)
            => camera.WorldToScreenPoint(pos);
        
        public static Vector2 WorldToScreen(this Vector2 pos, Camera camera) 
            => camera.WorldToScreenPoint(pos);


        public static Vector2 MergeWorldToCell(this Vector2 worldPos)
        {
            var xCell = Mathf.Round(worldPos.x / (1f / 2f));
            var even = xCell % 2 == 0;
            var yCell = even
                ? Mathf.Round((worldPos.y - CellHeight / 2f) / CellHeight) + 0.5f
                : Mathf.Round(worldPos.y / CellHeight); 
            return new Vector2(xCell, yCell);
        }

        
        public static Vector2 WorldToTileCenter(this Vector2 worldPos) =>
            MergeWorldToCell(worldPos) * new Vector2(0.5f, CellHeight);

       
        // public static Vector2 MergeRound(this Vector2 worldPos)
        // {
        //     worldPos.x =  Mathf.Round(worldPos.x / (1f/2f)) * (1f/2f); 
        //     var even = Mathf.Round(worldPos.x / (1f / 2f)) % 2 == 0;
        //     worldPos.y = even ? 
        //         (Mathf.Round((worldPos.y - 151f/256f/2f)  / (151f/256f) ) ) * (151f/256f) + 151f/256f/2f
        //         : (Mathf.Round(worldPos.y / (151f/256f)) ) * (151f/256f);
        //     return worldPos;
        // }
    }
}