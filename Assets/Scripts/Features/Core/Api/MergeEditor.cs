using Systems.Merge;
using Data;
using Sirenix.OdinInspector;
using UnityEngine;
using Utils;

namespace Core
{
    public class MergeEditor : SerializedMonoBehaviour
    {
        public Camera Camera;
        public Transform CameraTransform;
        public Transform Container;
        public Transform BgContainer;
        public EditorConfig EditorConfig;
        public MergeConfig MergeConfig;
        public GameObject BgPrefab;
        public MergeItemView Prefab;
        public GridGeneratorLinks GridGeneratorLinks;
        public LevelConfig Level;
        
        private ViewFactoryService _factory;
        private GridGeneratorApi _gridGenerator;

        [ShowInInspector]
        private CameraFitApi _cameraFit = new CameraFitApi();

        [Button]
        public void SetCtx()
        {
            _cameraFit.SetCtx(Camera, CameraTransform);
            _gridGenerator = new GridGeneratorApi(GridGeneratorLinks);
        }

        private void OnEnable()
        {
            SetCtx();
            Next();
        }

        private void OnDisable()
        {
            Clear();
        }

        public void LoadLevel(LevelConfig level)
        {
            Level = level;
            Reload();
        }

        [Button, HorizontalGroup("Buttons")]
        public void Prev()
        {
            var index = MergeConfig.Levels.IndexOf(Level) ;
            index--;
            if (index < 0) 
                index = MergeConfig.Levels.Count - 1;

            Level = MergeConfig.Levels[index];
            Reload();
        }

        [Button, HorizontalGroup("Buttons")]
        public void Next()
        {
            var index = MergeConfig.Levels.IndexOf(Level) ;
            index++;
            if (index >= MergeConfig.Levels.Count -1) 
                index = 0;
            
            Level = MergeConfig.Levels[index];
            Reload();
        }
        
        
        [Button, HorizontalGroup("Buttons")]
        public void Reload()
        {
            Clear();
            
            EditorConfig.Level = Level;
            
            _gridGenerator.LoadLevel(Level);
            _cameraFit.LoadLevel(Level);
            
            foreach (var itemData in EditorConfig.Level.Items)
                CreateItem(itemData);
        }

        private void Clear()
        {
            Container.ForEachReverse(child => Destroy(child.gameObject));
        }
        
        private void Update()
        {
            // if(EventSystem.current.IsPointerOverGameObject())
            //     return;
            
            if (Input.GetMouseButtonDown(1))
            {
                var unlock = Input.GetKey(KeyCode.LeftShift);
                UpdateLocked(unlock);
            }

            if (Input.GetMouseButtonDown(0))
            {
                if(EditorConfig.Selected == null)
                    return;
                if(EditorConfig.Selected is MergeItemConfig)
                    AddRemove( EditorConfig.Selected as MergeItemConfig);
                if(EditorConfig.Selected is MergeBgConfig)
                    AddRemoveBg( EditorConfig.Selected as MergeBgConfig);
            }

        }
        
        private void AddRemoveBg(MergeBgConfig mergeItemConfig)
        {
            var (posX, posY) = Camera.RoundMousePos();
            if(OutOfBounds(posX, posY))
                return;
            
            
            var levelItem = EditorConfig.Level.Holes.Find(v => v.x == posX && v.y == posY);
            if (levelItem == null)
            {
                EditorConfig.Level.Holes.Add(new V2(posX, posY));
                // CreateBgItem(new V2(posX, posY));
            }
            else
            {
                EditorConfig.Level.Holes.Remove(levelItem);
                // DestroyBgAtPos(posX, posY);
            }
            
            _gridGenerator.LoadLevel(Level);
        }

        private void AddRemove(MergeItemConfig mergeItemConfig)
        {
            var (posX, posY) = Camera.RoundMousePos();
            
            if(OutOfBounds(posX, posY))
                return;
            
            
            var levelItem = EditorConfig.Level.Items.Find(v => v.x == posX && v.y == posY);
            var hasItem = levelItem != null;
            if (!hasItem)
            {
                var data = new MergeItemProfileData(mergeItemConfig, posX, posY, 0);
                CreateItem(data);
                EditorConfig.Level.Items.Add(data);
            }
            else
            {
                EditorConfig.Level.Items.Remove(levelItem);
                DestroyAllChildsAtPos(posX, posY);
            }
        }

        private void DestroyBgAtPos(int posX, int posY)
        {
            BgContainer.ForEachReverse(child =>
            {
                var pos = child.transform.position;
                if (pos.x == posX && pos.y == posY)
                    Destroy(child.gameObject);
            });
        }  
        private void DestroyAllChildsAtPos(int posX, int posY)
        {
            Container.ForEachReverse(child =>
            {
                var item = child.GetComponent<MergeItemView>();
                if (item.Data.x == posX && item.Data.y == posY)
                {
                    Destroy(item.gameObject);
                }
            });
        }
        
        private void CreateItem(MergeItemProfileData itemData)
        {
            var item = Instantiate(Prefab, new Vector3(itemData.x, itemData.y, 0), Quaternion.identity, Container);
            item.SetCtx(itemData, i => { });
            item.ApplyFromData();
        }
 
        private void CreateBgItem(V2 itemData)
        {
            Instantiate(BgPrefab, new Vector3(itemData.x, itemData.y, 0), Quaternion.identity, BgContainer);
        }

        private void UpdateLocked(bool unlock)
        {
            var pos = Camera.ScreenToWorldPoint(Input.mousePosition);
            var posX = Mathf.RoundToInt(pos.x);
            var posY = Mathf.RoundToInt(pos.y);
            var levelItem = EditorConfig.Level.Items.Find(v => v.x == posX && v.y == posY);
            if(levelItem == null)
                return;
            
            if(OutOfBounds(posX, posY))
                return;

            Container.ForEachReverse(child =>
            {
                var item = child.GetComponent<MergeItemView>();
                if (item.Data.x == posX && item.Data.y == posY)
                {
                    item.Data.lockCount += unlock ? -1 : 1;
                    if (item.Data.lockCount < 0)
                        item.Data.lockCount = 0;
                    
                    item.ApplyFromData();
                    item.AnimateUnlocksEditor();
                    
                }
            });

        }

        private bool OutOfBounds(int posX, int posY) => 
            posX < 0 || posY < 0 || posX >= EditorConfig.Level.Width || posY >= EditorConfig.Level.Height;

    }

   
}