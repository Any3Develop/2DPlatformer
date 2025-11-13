using System;
using UI;
using UI.Service;
using UnityEngine;

[DefaultExecutionOrder(-999)]
public class GameInstaller : MonoBehaviour
{
    [SerializeField] private UIService uiService;
    private void Awake()
    {
        var input = new PlatformerInput();
        input.Enable();
        
        DIContainer.Instance.BindInstance(input);
        DIContainer.Instance.BindInstance(uiService);
    }

    private void Start()
    {
        // if (Application.isMobilePlatform) TODO: add platform check if needed
        uiService.Open<MobileInputWindow>();
    }

    private void OnDestroy()
    {
        DIContainer.Instance.UnbindInstance<PlatformerInput>();
        DIContainer.Instance.UnbindInstance<UIService>();
    }
}