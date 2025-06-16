using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Runtime.Core.Controllers;
using Runtime.Core.UI.Popup;
using Runtime.Game.Services.UI;
using Runtime.Game.Services.UserData.Data;

namespace Runtime.Game.GameStates.Game
{
    public class MarketItemSelectController : BaseController
    {
        private readonly IUiService _uiService;
        private readonly MarketItemSelectionFactory _marketItemSelectionFactory;

        public MarketItemSelectController(IUiService uiService, MarketItemSelectionFactory marketItemSelectionFactory)
        {
            _uiService = uiService;
            _marketItemSelectionFactory = marketItemSelectionFactory;
        }
        
        public event Action<ItemData> OnMarketItemSelect;

        public override async UniTask Run(CancellationToken cancellationToken)
        {
            MarketItemSelectPopup popup = await _uiService.ShowPopup(ConstPopups.MarketItemSelectPopup) as MarketItemSelectPopup;

            var selection = _marketItemSelectionFactory.GetItemSelection();
            popup.SetItems(selection);

            popup.OnClose += () =>
            {
                popup.DestroyPopup();
            };
            
            foreach (var item in selection)
            {
                item.OnSelect += data =>
                {
                    OnMarketItemSelect?.Invoke(data);
                    popup.DestroyPopup();
                };
            }
        }
    }
}