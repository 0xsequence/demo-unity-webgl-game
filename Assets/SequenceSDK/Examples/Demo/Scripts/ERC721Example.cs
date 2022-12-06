using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Numerics;
using Nethereum.Web3;
using NBitcoin;
using System.Text;

namespace SequenceSharp
{
    public class ERC721Example : MonoBehaviour
    {
        //Set in inspector

        public BigInteger tokenId = 1;
        public string contractAddress = "";
        public string accountAddress = "";

        public Wallet _wallet;


        /// <summary>
        /// Call after signing in sequence wallet
        /// </summary>

        public async void ERC721Examples()
        {
            _wallet = FindObjectOfType<Wallet>();
            var web3 = new Web3();
            web3.Client.OverridingRequestInterceptor = new SequenceInterceptor(_wallet, 137);
            var erc721 = new ERC721(web3, contractAddress);

            //Generate a random address for example testing
            var randomWallet = new Nethereum.HdWallet.Wallet(exampleWords, examplePassword);
            var randomAddress = randomWallet.GetAccount(0).Address;


            Debug.Log("[Sequence] ERC721 Token Example:");
            var name = await erc721.Name();
            Debug.Log("name: " + name);
            var symbol = await erc721.Symbol( );
            Debug.Log("symbol: " + symbol);
            var tokenURI = await erc721.TokenURI(tokenId);
            Debug.Log("tokenURI: " + tokenURI);
            accountAddress = await _wallet.GetAddress();
            var balanceOf = await erc721.BalanceOf(accountAddress);
            Debug.Log("balanceOf: " + balanceOf);
            var ownerOf = await erc721.OwnerOf(tokenId);
            Debug.Log("ownerOf: " + ownerOf);

            //Tests for transaction functions:
            await erc721.SafeTransferFrom(accountAddress, randomAddress, tokenId);
            //await erc721.TransferFrom(accountAddress, randomAddress, tokenId);
            //await erc721.Approve(randomAddress, tokenId);
            //var receipt = await erc721.GetApproved(tokenId);
            //var receipt = await erc721.SetApprovalForAll(accountAddress, true);
            //var receipt = await erc721.IsApprovedForAll(accountAddress, randomAddress);
            
            
            //Debug.Log("receipt from GetApproved: " + receipt);
        }

        private static Mnemonic exampleMnemo = new Mnemonic(Wordlist.English, WordCount.Twelve);
        private static string exampleWords = exampleMnemo.ToString(); // "ripple scissors kick mammal hire column oak again sun offer wealth tomorrow wagon turn fatal"
        private static string examplePassword = "password";
    }
}