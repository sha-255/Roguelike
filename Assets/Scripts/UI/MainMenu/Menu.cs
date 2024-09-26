using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

[RequireComponent(typeof(UIDocument))]
public class Menu : MonoBehaviour
{
    [SerializeField] private List<MenuEntry> _menuEntries;
    [Header("Animation")]
    [SerializeField] private float _transitionDuration = 0.5f;
    [SerializeField] private EasingMode _easing = EasingMode.EaseOutBounce;
    [SerializeField] private float _buttonDelay = 0.3f;
    [Header("Components")]
    [SerializeField] VisualTreeAsset _buttonTemplate;
    [SerializeField] private string _animatedClass = "animate-slide-left";
    [SerializeField] string _containerElement = "container";
    [SerializeField] string _buttonElement = "menu-button";
    private VisualElement _container;
    private WaitForSeconds _pause;
    private List<TimeValue> _timeValues;
    private StyleList<EasingFunction> _transition;

    private void OnValidate()
    {
        _pause = new WaitForSeconds(_buttonDelay);
        _timeValues = new List<TimeValue>()
        {
            new TimeValue(_transitionDuration, TimeUnit.Second)
        };
        _transition = ApplyTransitions();
    }

    private void Awake()
    {
        _container = GetComponent<UIDocument>().rootVisualElement.Q<VisualElement>(_containerElement);
        StartCoroutine(CreateMenu());
    }

    private IEnumerator CreateMenu()
    {
        foreach (var entry in _menuEntries)
        {
            var newElement = _buttonTemplate.CloneTree();
            var button = newElement.Q<Button>(_buttonElement);
            button.Q<Label>("label").text = entry.Name;
            button.clicked += delegate { Click(entry); };
            _container.Add(newElement);
            newElement.style.transitionDuration = _timeValues;
            newElement.style.transitionTimingFunction = _transition;
            newElement.AddToClassList(_animatedClass);
            yield return null;
            newElement.RemoveFromClassList(_animatedClass);
            yield return _pause;
        }
    }

    private StyleList<EasingFunction> ApplyTransitions()
    {
        var easyncFunc = new List<EasingFunction>() { new EasingFunction(_easing) };
        var StyleList = new StyleList<EasingFunction>();
        StyleList.value = easyncFunc;
        return StyleList;
    }

    private void Click(MenuEntry entry)
    {
        print("ButtonCl" + entry.Name);
        entry.OnClick.Invoke();
    }
}

[System.Serializable]
public class MenuEntry
{
    public string Name;
    public UnityEvent OnClick;
}