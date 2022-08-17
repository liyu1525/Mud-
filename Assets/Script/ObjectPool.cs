using UnityEngine;
namespace HotFix_Project
{
    internal class ObjectPool
    {
        public static Transform PoolTran;
        private GameObject Pool;
        private GameObject CoPy_Obj = null;
        private int Count = 0;
        private System.Action<GameObject> m_Recycle;


        /// <summary>
        /// ���캯������ʼ�������
        /// </summary>
        /// <param name="copy"></param>
        /// <param name="count"></param>
        public ObjectPool(string name, GameObject copy, int count)
        {
            CoPy_Obj = copy;
            Count = count;

            Pool = new GameObject(name);
            if (PoolTran)
                Pool.transform.parent = PoolTran;
            Pool.SetActive(false);
        }

        public void Set_Recycle(System.Action<GameObject> k)
        {
            m_Recycle = k;
        }

        /// <summary>
        /// �Ӷ�����л�ȡһ������
        /// </summary>
        /// <returns></returns>
        public GameObject Get()
        {
            GameObject obj;
            if (Pool.transform.childCount > 0)
                obj = Pool.transform.GetChild(Pool.transform.childCount - 1).gameObject;
            else
            {
                obj = Object.Instantiate(CoPy_Obj);
            }

            if (!obj.activeInHierarchy)
                obj.SetActive(true);
            obj.name = "item";
            return obj;
        }


        /// <summary>
        /// ��һ������Ž������
        /// </summary>
        /// <param name="obj"></param>
        public void Put(GameObject obj)
        {
            if (Pool.transform.childCount <= Count)
            {
                if (m_Recycle != null)
                    m_Recycle(obj);
                obj.transform.SetParent(Pool.transform, false);
            }
            else
            {
                Object.Destroy(obj);
            }
        }

        /// <summary>
        /// ��һ������Ž������
        /// </summary>
        /// <param name="obj"></param>
        public void Put(Transform obj)
        {
            if (Pool.transform.childCount <= Count)
            {
                if (m_Recycle != null)
                    m_Recycle(obj.gameObject);
                obj.transform.SetParent(Pool.transform, false);
            }
            else
            {
                Object.Destroy(obj);
            }
        }

        /// <summary>
        /// �Ѳ�������Ӷ���ȫ�ŵ��������
        /// </summary>
        /// <param name="parent"></param>
        public void Put_All(Transform parent)
        {
            GameObject obj;
            if (!parent)
                return;
            for (int i = parent.childCount; i > 0; i--)
            {
                obj = parent.GetChild(i - 1).gameObject;
                if (Pool.transform.childCount <= Count)
                {
                    if (m_Recycle != null)
                        m_Recycle(obj);
                    obj.transform.SetParent(Pool.transform, false);
                }
                else
                {
                    Object.Destroy(obj);
                }
            }

        }
    }
}

