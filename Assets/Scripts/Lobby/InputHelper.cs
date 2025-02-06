using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.InputSystem;
using TMPro;

public class InputHelper : MonoBehaviour
{

    public static EventHandler<InputAction.CallbackContext> MoveEvent;
    public static EventHandler<InputAction.CallbackContext> SideBreakEvent;
    public static EventHandler<InputAction.CallbackContext> EscapeEvent;

    public static EventHandler<InputAction.CallbackContext> WorldmapEvent;
    public static EventHandler<InputAction.CallbackContext> WorldmapZoomEvent;
    public static EventHandler<InputAction.CallbackContext> WorldmapMoveEvent;

    public static EventHandler<InputAction.CallbackContext> EnterStoreEvent;

    public static EventHandler<InputAction.CallbackContext> TabMoveEvent;

    public static EventHandler<InputAction.CallbackContext> UIMoveEvent;
    public static EventHandler<InputAction.CallbackContext> OkayEvent;
    public static EventHandler<InputAction.CallbackContext> BackEvent;

    public static EventHandler<InputAction.CallbackContext> FastTravelEvent;

    // playerInput => Event에 아래 메소드 추가

    public void OnMove(InputAction.CallbackContext context)
    {
        if (SteamWishlist.SteamOverlayActivated) return;

        MoveEvent?.Invoke(null, context);
    }

    public void OnSideBreak(InputAction.CallbackContext context)
    {
        if (SteamWishlist.SteamOverlayActivated) return;

        SideBreakEvent?.Invoke(null, context);
    }

    public void OnEscape(InputAction.CallbackContext context)
    {
        if (SteamWishlist.SteamOverlayActivated) return;

        EscapeEvent?.Invoke(null, context);
    }

    public void OnWorldmap(InputAction.CallbackContext context)
    {
        if (SteamWishlist.SteamOverlayActivated) return;

        WorldmapEvent?.Invoke(null, context);
    }

    public void OnWorldmapZoom(InputAction.CallbackContext context)
    {
        if (SteamWishlist.SteamOverlayActivated) return;

        WorldmapZoomEvent?.Invoke(null, context);
    }

    public void OnWorldmapMove(InputAction.CallbackContext context)
    {
        if (SteamWishlist.SteamOverlayActivated) return;

        WorldmapMoveEvent?.Invoke(null, context);
    }

    public void OnEnterStore(InputAction.CallbackContext context)
    {
        if (SteamWishlist.SteamOverlayActivated) return;

        EnterStoreEvent?.Invoke(null, context);
    }

    public void OnTabMove(InputAction.CallbackContext context)
    {
        if (SteamWishlist.SteamOverlayActivated) return;

        TabMoveEvent?.Invoke(null, context);
    }

    public void OnUIMove(InputAction.CallbackContext context)
    {
        if (SteamWishlist.SteamOverlayActivated) return;

        UIMoveEvent?.Invoke(null, context);
    }
    public void OnOkay(InputAction.CallbackContext context)
    {
        if (SteamWishlist.SteamOverlayActivated) return;

        OkayEvent?.Invoke(null, context);
    }
    public void OnBack(InputAction.CallbackContext context)
    {
        if (SteamWishlist.SteamOverlayActivated) return;

        BackEvent?.Invoke(null, context);
    }

    public void OnFastTravel(InputAction.CallbackContext context)
    {
        if (SteamWishlist.SteamOverlayActivated) return;

        FastTravelEvent?.Invoke(null, context);
    }

    public void PadDisconnected()
    {
        var um = UIManager.Instance;
        if (um != null)
        {
            //if (um.Panels_Inactive)
            //{
            //    um.utilUI.OpenSettings();
            //}
        }
        Cursor.visible = true;
    }
}
