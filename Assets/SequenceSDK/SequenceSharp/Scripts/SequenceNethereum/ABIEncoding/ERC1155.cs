using System.Numerics;
using System.Threading.Tasks;
using System;
using UnityEngine;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace SequenceSharp
{
    public struct ERC1155Balance
    {
        public string type;
        public string hex;
    }
    public class ERC1155 : MonoBehaviour
    {
        private static string abi = "[ { \"inputs\": [ { \"internalType\": \"string\", \"name\": \"uri_\", \"type\": \"string\" } ], \"stateMutability\": \"nonpayable\", \"type\": \"constructor\" }, { \"anonymous\": false, \"inputs\": [ { \"indexed\": true, \"internalType\": \"address\", \"name\": \"account\", \"type\": \"address\" }, { \"indexed\": true, \"internalType\": \"address\", \"name\": \"operator\", \"type\": \"address\" }, { \"indexed\": false, \"internalType\": \"bool\", \"name\": \"approved\", \"type\": \"bool\" } ], \"name\": \"ApprovalForAll\", \"type\": \"event\" }, { \"anonymous\": false, \"inputs\": [ { \"indexed\": true, \"internalType\": \"address\", \"name\": \"operator\", \"type\": \"address\" }, { \"indexed\": true, \"internalType\": \"address\", \"name\": \"from\", \"type\": \"address\" }, { \"indexed\": true, \"internalType\": \"address\", \"name\": \"to\", \"type\": \"address\" }, { \"indexed\": false, \"internalType\": \"uint256[]\", \"name\": \"ids\", \"type\": \"uint256[]\" }, { \"indexed\": false, \"internalType\": \"uint256[]\", \"name\": \"values\", \"type\": \"uint256[]\" } ], \"name\": \"TransferBatch\", \"type\": \"event\" }, { \"anonymous\": false, \"inputs\": [ { \"indexed\": true, \"internalType\": \"address\", \"name\": \"operator\", \"type\": \"address\" }, { \"indexed\": true, \"internalType\": \"address\", \"name\": \"from\", \"type\": \"address\" }, { \"indexed\": true, \"internalType\": \"address\", \"name\": \"to\", \"type\": \"address\" }, { \"indexed\": false, \"internalType\": \"uint256\", \"name\": \"id\", \"type\": \"uint256\" }, { \"indexed\": false, \"internalType\": \"uint256\", \"name\": \"value\", \"type\": \"uint256\" } ], \"name\": \"TransferSingle\", \"type\": \"event\" }, { \"anonymous\": false, \"inputs\": [ { \"indexed\": false, \"internalType\": \"string\", \"name\": \"value\", \"type\": \"string\" }, { \"indexed\": true, \"internalType\": \"uint256\", \"name\": \"id\", \"type\": \"uint256\" } ], \"name\": \"URI\", \"type\": \"event\" }, { \"inputs\": [ { \"internalType\": \"address\", \"name\": \"account\", \"type\": \"address\" }, { \"internalType\": \"uint256\", \"name\": \"id\", \"type\": \"uint256\" } ], \"name\": \"balanceOf\", \"outputs\": [ { \"internalType\": \"uint256\", \"name\": \"\", \"type\": \"uint256\" } ], \"stateMutability\": \"view\", \"type\": \"function\" }, { \"inputs\": [ { \"internalType\": \"address[]\", \"name\": \"accounts\", \"type\": \"address[]\" }, { \"internalType\": \"uint256[]\", \"name\": \"ids\", \"type\": \"uint256[]\" } ], \"name\": \"balanceOfBatch\", \"outputs\": [ { \"internalType\": \"uint256[]\", \"name\": \"\", \"type\": \"uint256[]\" } ], \"stateMutability\": \"view\", \"type\": \"function\" }, { \"inputs\": [ { \"internalType\": \"address\", \"name\": \"account\", \"type\": \"address\" }, { \"internalType\": \"address\", \"name\": \"operator\", \"type\": \"address\" } ], \"name\": \"isApprovedForAll\", \"outputs\": [ { \"internalType\": \"bool\", \"name\": \"\", \"type\": \"bool\" } ], \"stateMutability\": \"view\", \"type\": \"function\" }, { \"inputs\": [ { \"internalType\": \"address\", \"name\": \"_address\", \"type\": \"address\" }, { \"internalType\": \"uint256\", \"name\": \"_amount\", \"type\": \"uint256\" } ], \"name\": \"ownerMint\", \"outputs\": [], \"stateMutability\": \"nonpayable\", \"type\": \"function\" }, { \"inputs\": [ { \"internalType\": \"address\", \"name\": \"from\", \"type\": \"address\" }, { \"internalType\": \"address\", \"name\": \"to\", \"type\": \"address\" }, { \"internalType\": \"uint256[]\", \"name\": \"ids\", \"type\": \"uint256[]\" }, { \"internalType\": \"uint256[]\", \"name\": \"amounts\", \"type\": \"uint256[]\" }, { \"internalType\": \"bytes\", \"name\": \"data\", \"type\": \"bytes\" } ], \"name\": \"safeBatchTransferFrom\", \"outputs\": [], \"stateMutability\": \"nonpayable\", \"type\": \"function\" }, { \"inputs\": [ { \"internalType\": \"address\", \"name\": \"from\", \"type\": \"address\" }, { \"internalType\": \"address\", \"name\": \"to\", \"type\": \"address\" }, { \"internalType\": \"uint256\", \"name\": \"id\", \"type\": \"uint256\" }, { \"internalType\": \"uint256\", \"name\": \"amount\", \"type\": \"uint256\" }, { \"internalType\": \"bytes\", \"name\": \"data\", \"type\": \"bytes\" } ], \"name\": \"safeTransferFrom\", \"outputs\": [], \"stateMutability\": \"nonpayable\", \"type\": \"function\" }, { \"inputs\": [ { \"internalType\": \"address\", \"name\": \"operator\", \"type\": \"address\" }, { \"internalType\": \"bool\", \"name\": \"approved\", \"type\": \"bool\" } ], \"name\": \"setApprovalForAll\", \"outputs\": [], \"stateMutability\": \"nonpayable\", \"type\": \"function\" }, { \"inputs\": [ { \"internalType\": \"bytes4\", \"name\": \"interfaceId\", \"type\": \"bytes4\" } ], \"name\": \"supportsInterface\", \"outputs\": [ { \"internalType\": \"bool\", \"name\": \"\", \"type\": \"bool\" } ], \"stateMutability\": \"view\", \"type\": \"function\" }, { \"inputs\": [ { \"internalType\": \"uint256\", \"name\": \"\", \"type\": \"uint256\" } ], \"name\": \"uri\", \"outputs\": [ { \"internalType\": \"string\", \"name\": \"\", \"type\": \"string\" } ], \"stateMutability\": \"view\", \"type\": \"function\" } ]";

        public static Wallet _wallet;
        private void Awake()
        {
            _wallet = FindObjectOfType<Wallet>();
        }
        public static async Task<string> URI(BigInteger _id, string address, int chainId)
        {
            //throw new NotImplementedException();
            string URI = await _wallet.ExecuteSequenceJS(@"
                const wallet = seq.getWallet();           
                const networks = await wallet.getNetworks();
                const n = networks.find(n => n['chainId']==" + chainId + @");
                const signer = wallet.getSigner(n);
                const abi =" + abi + @";
                const erc1155 = new ethers.Contract('" + address + @"', abi, signer); 

                var uri = await erc1155.uri(" + _id+ @");
                return uri;

            ");

            return URI;
        }

        public static async Task<BigInteger> BalanceOf(string account, BigInteger id, string address, int chainId)
        {
            // throw new NotImplementedException();
            string balanceOf = await _wallet.ExecuteSequenceJS(@"
                const wallet = seq.getWallet();           
                const networks = await wallet.getNetworks();
                const n = networks.find(n => n['chainId']==" + chainId + @");
                const signer = wallet.getSigner(n);
                const abi =" + abi + @";
                const erc1155 = new ethers.Contract('" + address + @"', abi, signer); 

                var balanceOf = await erc1155.balanceOf('" + account+"',"+ id + @");
                return balanceOf;

            ");
            ERC1155Balance balanceOfParsed = JsonConvert.DeserializeObject<ERC1155Balance>(balanceOf);
            
            return BigInteger.Parse(balanceOfParsed.hex.Substring(2), System.Globalization.NumberStyles.HexNumber);
        }

        
        public static async Task<BigInteger[]> BalanceOfBatch(string[] accounts, BigInteger[] ids, string address, int chainId)
        {

            //throw new NotImplementedException();
            string accountsJS = "[";
            foreach(var acc in accounts)
            {
                accountsJS += "'"+ acc+"'" +",";
            }
            accountsJS = accountsJS.Remove(accountsJS.Length - 1, 1);
            accountsJS += "]";

            string idsJS = "[";
            foreach (var id in ids)
            {
                
                idsJS += id.ToString() + ",";
            }
            idsJS = idsJS.Remove(idsJS.Length - 1, 1);
            idsJS += "]";
 


            var balanceOfBatchRes = await _wallet.ExecuteSequenceJS(@"
                const wallet = seq.getWallet();           
                const networks = await wallet.getNetworks();
                const n = networks.find(n => n['chainId']==" + chainId + @");
                const signer = wallet.getSigner(n);
                const abi =" + abi + @";
                const erc1155 = new ethers.Contract('" + address + @"', abi, signer); 

                var balanceOfBatch = await erc1155.balanceOfBatch(" + accountsJS + "," + idsJS + @");
                
                console.log(balanceOfBatch);
                return balanceOfBatch;

            ");

            JArray balanceOfBatchList = JArray.Parse(balanceOfBatchRes);
            
            BigInteger[] balanceOfBatch = new BigInteger[ids.Length];
            int j = 0;
            foreach (JObject balanceOf in balanceOfBatchList)
            {
                
                
                ERC1155Balance balanceOfParsed = JsonConvert.DeserializeObject<ERC1155Balance>(balanceOf.ToString());
                balanceOfBatch[j++] = BigInteger.Parse(balanceOfParsed.hex.Substring(2), System.Globalization.NumberStyles.HexNumber);
                
            }
            
            return balanceOfBatch;
        }

        public static async Task SetApprovalForAll(string operatorAddress, bool approved)
        {
            throw new NotImplementedException();
        }
        public static async Task<bool> IsApprovedForAll(string account, string operatorAddress)
        {
            throw new NotImplementedException();
        }
        public static async Task SafeTransferFrom(string from, string to, BigInteger id, BigInteger amount, string data)
        {
            throw new NotImplementedException();
        }
        public static async Task SafeBatchTransferFrom(string from, string to, BigInteger[] ids, BigInteger[] amounts, string data)
        {
            throw new NotImplementedException();
        }
    }
}