﻿namespace DownUnder.Input
{
    /// <summary>
    /// User defined actions. Used to request input.
    /// </summary>
    public enum ActionType
    {
        error,
        menu_select,
        menu_back,
        open_menu,
        menu_close,
        menu_select_up,
        menu_select_down,
        menu_select_left,
        menu_select_right,
        menu_select_speed_modifier,
        camera_zoom_out,
        camera_zoom_in,
        up_movement,
        down_movement,
        shield,
        jump,
        neutral_attack,
        light_up_attack,
        light_down_attack,
        light_left_attack,
        light_right_attack,
        charge_up_attack,
        charge_down_attack,
        charge_left_attack,
        charge_right_attack,
        quick_charge_up_attack,
        quick_charge_down_attack,
        quick_charge_left_attack,
        quick_charge_right_attack,
        left_movement,
        right_movement
    }
}