using System;
using System.Reactive.Linq;

using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace BMBot.GUI.Avalonia.Models.DataStructures.Game.Instance;

public class GameData : ReactiveObject
{
    public GameData()
    {
        this.WhenAny(p_x => p_x.CharacterScreenIsOpen, p_x => p_x.Value)
            .CombineLatest(this.WhenAny(p_x => p_x.ShopScreenIsOpen, p_x => p_x.Value),
                           this.WhenAny(p_x => p_x.QuestScreenIsOpen, p_x => p_x.Value),
                           this.WhenAny(p_x => p_x.MercInventoryScreenIsOpen, p_x => p_x.Value),
                           this.WhenAny(p_x => p_x.ImbueScreenIsOpen, p_x => p_x.Value),
                           this.WhenAny(p_x => p_x.StashScreenIsOpen, p_x => p_x.Value),
                           this.WhenAny(p_x => p_x.CubeScreenIsOpen, p_x => p_x.Value),
                           this.WhenAny(p_x => p_x.PartyScreenIsOpen, p_x => p_x.Value),
                           this.WhenAny(p_x => p_x.WaypointScreenIsOpen, p_x => p_x.Value),
                           (p_property1, p_property2, p_property3, p_property4, p_property5, p_property6, p_property7, p_property8, p_property9) =>
                               new
                               {
                                   property1 = p_property1,
                                   property2 = p_property2,
                                   property3 = p_property3,
                                   property4 = p_property4,
                                   property5 = p_property5,
                                   property6 = p_property6,
                                   property7 = p_property7,
                                   property8 = p_property8,
                                   property9 = p_property9
                               })
            .Subscribe(p_result =>
                       {
                           LeftPanelIsOpen = p_result.property1 ||
                                             p_result.property2 ||
                                             p_result.property3 ||
                                             p_result.property4 ||
                                             p_result.property5 ||
                                             p_result.property6 ||
                                             p_result.property7 ||
                                             p_result.property8 ||
                                             p_result.property9;
                       });
        
        this.WhenAnyValue(p_data => p_data.InventoryIsOpen,
                          p_data => p_data.SkillTreeIsOpen)
            .Subscribe(p_values =>
                       {
                           RightPanelIsOpen = p_values.Item1 || p_values.Item2;
                       });
    }

    [Reactive] public bool GameSessionIsActive { get; set; }
    
    [Reactive] public bool MercIsActive { get; set; }

    #region UI Elements

    [Reactive] public bool MiniMapIsEnabled { get; set; }

    [Reactive] public bool GameMenuIsOpen      { get; set; }
    [Reactive] public bool HelpScreenIsOpen    { get; set; }
    [Reactive] public bool ChatPanelIsOpen     { get; set; }
    [Reactive] public bool NpcDialogIsActive   { get; set; }
    [Reactive] public bool PortraitsAreEnabled { get; set; }
    [Reactive] public bool SkillSelectorIsOpen { get; set; }
    [Reactive] public bool BeltIsExpanded      { get; set; }

    #region Left Panels

    [Reactive] public bool LeftPanelIsOpen { get; set; }

    [Reactive] public bool CharacterScreenIsOpen     { get; set; }
    [Reactive] public bool ShopScreenIsOpen          { get; set; }
    [Reactive] public bool QuestScreenIsOpen         { get; set; }
    [Reactive] public bool MercInventoryScreenIsOpen { get; set; }
    [Reactive] public bool ImbueScreenIsOpen         { get; set; }
    [Reactive] public bool StashScreenIsOpen         { get; set; }
    [Reactive] public bool CubeScreenIsOpen          { get; set; }
    [Reactive] public bool PartyScreenIsOpen         { get; set; }
    [Reactive] public bool WaypointScreenIsOpen      { get; set; }

    // TODO: Duriel staff screen? - Comment by M9 on 07/25/2024 @ 00:00:00

    #endregion

    #region Right Panels

    [Reactive] public bool RightPanelIsOpen { get; set; }
    
    [Reactive] public bool InventoryIsOpen { get; set; }
    [Reactive] public bool SkillTreeIsOpen { get; set; }

    #endregion

    #endregion
}