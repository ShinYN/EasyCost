using EasyCost.Databases.TableModels;
using EasyCost.DataModels;
using EasyCost.Helpers;
using System;
using System.Collections.Generic;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// 빈 페이지 항목 템플릿에 대한 설명은 https://go.microsoft.com/fwlink/?LinkId=234238에 나와 있습니다.

namespace EasyCost.Pages.Settings
{
    /// <summary>
    /// 자체적으로 사용하거나 프레임 내에서 탐색할 수 있는 빈 페이지입니다.
    /// </summary>
    public sealed partial class CardManagementPage : Page
    {
        public CardManagementPage()
        {
            this.InitializeComponent();

            InitControls();
            DisplayCardList();
        }

        private List<CardMaster> _cardList;

        private void InitControls()
        {

        }

        private void InitCardInfoControls()
        {
            txtCardName.Text = string.Empty;
            rbCreditCard.IsChecked = true;
            txtCardCompany.Text = string.Empty;
            txtCardNumber.Text = string.Empty;
            txtDescription.Text = string.Empty;

            txtCardName.IsEnabled = true;
            btnAddCard.Visibility = Visibility.Visible;
            btnAddCardContinue.Visibility = Visibility.Visible;
            btnUpdateCard.Visibility = Visibility.Collapsed;
            btnDeleteCard.Visibility = Visibility.Collapsed;

            txtCardName.Focus(FocusState.Keyboard);
        }

        private void DisplayCardList()
        {
            lsvCard.Items.Clear();
            _cardList = CardManager.GetCardList();

            if (_cardList == null)
            {
                return;
            }

            var index = 1;
            _cardList.ForEach(x =>
            {
                lsvCard.Items.Add(new CardItemModel
                {
                    Index = index,
                    CardName = x.CardName,
                    CardType = x.CardType,
                    Company = x.Company,
                    CardNumber = x.CardNumber,
                    Description = x.Description
                });

                index++;
            });
        }

        private void DisplayCardInfoForUpdate(CardItemModel aCardItemModel)
        {
            txtCardName.Text = aCardItemModel.CardName;
            if (aCardItemModel.CardType == "신용카드")
            {
                rbCreditCard.IsChecked = true;
            }
            else
            {
                rbDebitCard.IsChecked = true;
            }

            txtCardCompany.Text = aCardItemModel.Company;
            txtCardNumber.Text = aCardItemModel.CardName;
            txtDescription.Text = aCardItemModel.Description;

            txtCardName.IsEnabled = false;
            btnAddCard.Visibility = Visibility.Collapsed;
            btnAddCardContinue.Visibility = Visibility.Collapsed;
            btnUpdateCard.Visibility = Visibility.Visible;
            btnDeleteCard.Visibility = Visibility.Visible;

            if (cardInfoSplitView.IsPaneOpen == false)
            {
                cardInfoSplitView.IsPaneOpen = true;
            }
        }

        private void SaveCardMaster()
        {
            CheckIsValidCardInfoData();
            CardManager.SaveCardMaster(MakeCardMasterData());
        }

        private void UpdateCardMaster()
        {
            CheckIsValidCardInfoData();
            CardManager.UpdateCardMaster(MakeCardMasterData());
        }

        private void DeleteCardMaster()
        {
            if (lsvCard.SelectedItem != null)
            {
                var deleteItem = (CardItemModel)lsvCard.SelectedItem;

                CardManager.DeleteCardMaster(deleteItem.CardName);
                CostManager.UpdateCostInfoCardName(deleteItem.CardName, string.Empty);
            }
        }

        private void CheckIsValidCardInfoData()
        {
            if (string.IsNullOrWhiteSpace(txtCardName.Text))
            {
                txtCardName.Focus(FocusState.Keyboard);
                throw new Exception("카드 이름을 입력해 주세요");
            }

            if (!rbCreditCard.IsChecked.Value && !rbDebitCard.IsChecked.Value)
            {
                rbCreditCard.Focus(FocusState.Keyboard);
                throw new Exception("카드 타입을 선택해 주세요");
            }

            if (string.IsNullOrWhiteSpace(txtCardCompany.Text))
            {
                txtCardCompany.Focus(FocusState.Keyboard);
                throw new Exception("카드 회사를 입력해 주세요");
            }
        }

        private CardMaster MakeCardMasterData()
        {
            CardMaster cardMaster = new CardMaster()
            {
                CardName = txtCardName.Text.Trim(),
                CardType = (rbCreditCard.IsChecked.Value) ? "신용카드" : "체크카드",
                Company = txtCardCompany.Text.Trim(),
                CardNumber = txtCardNumber.Text.Trim(),
                Description = txtDescription.Text.Trim()
            };
           
            return cardMaster;
        }

        private void lsvCard_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (e.ClickedItem != null && e.ClickedItem.GetType() == typeof(CardItemModel))
            {
                DisplayCardInfoForUpdate((CardItemModel)e.ClickedItem);
            }
        }

        private void btnAddNewCard_Click(object sender, RoutedEventArgs e)
        {
            InitCardInfoControls();
            cardInfoSplitView.IsPaneOpen = !cardInfoSplitView.IsPaneOpen;
        }

        private async void btnAddCard_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                SaveCardMaster();
                cardInfoSplitView.IsPaneOpen = false;
                DisplayCardList();
            }
            catch (Exception ex)
            {
                var dialog = new MessageDialog(ex.Message, "확인");
                dialog.Commands.Add(new UICommand { Label = "OK", Id = 0 });
                await dialog.ShowAsync();
            }
        }

        private async void btnAddCardContinue_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                SaveCardMaster();
                DisplayCardList();
                cardInfoSplitView.IsPaneOpen = true;
                InitCardInfoControls();
            }
            catch (Exception ex)
            {
                var dialog = new MessageDialog(ex.Message, "확인");
                dialog.Commands.Add(new UICommand { Label = "OK", Id = 0 });
                await dialog.ShowAsync();
            }
        }

        private async void btnDeleteCard_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                DeleteCardMaster();
                cardInfoSplitView.IsPaneOpen = false;
                DisplayCardList();
            }
            catch (Exception ex)
            {
                var dialog = new MessageDialog(ex.Message, "확인");
                dialog.Commands.Add(new UICommand { Label = "OK", Id = 0 });
                await dialog.ShowAsync();
            }
        }

        private async void btnUpdateCard_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                UpdateCardMaster();
                cardInfoSplitView.IsPaneOpen = false;
                DisplayCardList();
            }
            catch (Exception ex)
            {
                var dialog = new MessageDialog(ex.Message, "확인");
                dialog.Commands.Add(new UICommand { Label = "OK", Id = 0 });
                await dialog.ShowAsync();
            }
        }
    }
}
