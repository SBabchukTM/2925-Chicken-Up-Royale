using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Runtime.Core.Controllers;
using Runtime.Core.UI.Popup;
using Runtime.Game.Services.UI;
using Runtime.Game.Services.UserData.Data;

namespace Runtime.Game.Incubation
{
    public class EggItemSelectController : BaseController
    {
        private readonly IUiService _uiService;
        private readonly EggItemSelectFactory _eggItemSelectionFactory;

        public EggItemSelectController(IUiService uiService, EggItemSelectFactory eggItemSelectFactory)
        {
            _uiService = uiService;
            _eggItemSelectionFactory = eggItemSelectFactory;
        }
        
        public event Action<ItemData> OnEggSelect;

        public override async UniTask Run(CancellationToken cancellationToken)
        {
            MarketItemSelectPopup popup = await _uiService.ShowPopup(ConstPopups.MarketItemSelectPopup) as MarketItemSelectPopup;

            var selection = _eggItemSelectionFactory.GetItemSelection();
            popup.SetItems(selection);

            popup.OnClose += () =>
            {
                popup.DestroyPopup();
            };
            
            foreach (var item in selection)
            {
                item.OnSelect += data =>
                {
                    OnEggSelect?.Invoke(data);
                    popup.DestroyPopup();
                };
            }
        }
    }
}