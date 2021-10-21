using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Saves
{
    public sealed class AddressablesController : MonoBehaviour
    {
        [SerializeField]
        private string label;
        private Transform _parent;
        private List<GameObject> CreatedObjs { get; } = new List<GameObject>();

        private void Start()
        {
            _parent = GameObject.Find("Example Assets").transform;
            Instantiate();
        }

        private async void Instantiate()
        {
            await AddressablesLoader.InitAssets(label, CreatedObjs, _parent);
        }
    }
    
    public static class AddressablesLoader
    {
        public static async Task InitAssets<T>(string label, List<T> createdObjs, Transform parent) where T : Object
        {
            var locations = await Addressables.LoadResourceLocationsAsync(label).Task;

            foreach (var location in locations)
            {
                createdObjs.Add(await Addressables.InstantiateAsync(location, parent).Task as T);
            }
        }
    }
}