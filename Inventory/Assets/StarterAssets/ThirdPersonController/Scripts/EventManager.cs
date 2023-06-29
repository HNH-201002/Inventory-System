using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class EventManager {
    public delegate void MenuOpenCloseEventHandler(bool isOpen);
    public static event MenuOpenCloseEventHandler OnMenuOpenClose;
}
