using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WalletService.Dtos;

namespace WalletService.AsyncDataService
{
    public interface IMessageClient
    {
        void PublishNewWallet(WalletPublishDto walletPublishDto);
        void PublishTopupWallet(TopupWalletPublishDto topupWalletPublishDto);
    }
}