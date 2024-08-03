using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics; // For BigInteger
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using Nethereum.Web3;
using Nethereum.Contracts;
using Nethereum.ABI.FunctionEncoding.Attributes;
using TMPro;
using UnityEngine.Networking;
using Monaverse.Api.Modules.Collectibles.Dtos;





public class MainController : MonoBehaviour
{
    private Web3 web3;
    private Contract contract;
    private string contractAddress = "0x5ddb5d32a84ece5cdbd519714c3bf2c586daaffb";
    private string rpcUrl = "https://polygon-amoy.infura.io/v3/4764f6f6a4bb4aae8672c9d2627d0b05";
    private ArtworkRegistryService artworkRegistryService;
    private List<ArtworkRegistryService.ArtworkDTO> artworks;
    public TMP_Text artworkText;

    public MeshColliderAndXRGrabAdder meshColliderAndXRGrabAdder;

    void Start()
    {
        artworkRegistryService = new ArtworkRegistryService(rpcUrl, contractAddress);
        artworks = new List<ArtworkRegistryService.ArtworkDTO>();
        StartCoroutine(FetchArtworksCoroutine());
    }

    public List<ArtworkRegistryService.ArtworkDTO> GetArtworks()
    {
        return artworks;
    }

    IEnumerator FetchArtworksCoroutine()
    {
        var getArtworkCountTask = artworkRegistryService.GetArtworkCountAsync();
        yield return new WaitUntil(() => getArtworkCountTask.IsCompleted);

        if (getArtworkCountTask.Exception != null)
        {
            Debug.LogError("Error fetching artwork count: " + getArtworkCountTask.Exception);
            yield break;
        }

        BigInteger artworkCount = getArtworkCountTask.Result;
        Debug.Log($"Total Artworks: {artworkCount}");

        for (BigInteger i = 0; i < artworkCount; i++)
        {
            var getArtworkTask = artworkRegistryService.GetArtworkAsync(i);
            yield return new WaitUntil(() => getArtworkTask.IsCompleted);

            if (getArtworkTask.Exception != null)
            {
                Debug.LogError($"Error fetching artwork {i}: " + getArtworkTask.Exception);
                continue;
            }

            var artwork = getArtworkTask.Result;
            artworks.Add(artwork);
            Debug.Log($"Fetched Artwork {i}: {artwork.Name} {artwork.FileUrl} ({artwork.X}, {artwork.Y}, {artwork.Z}) {artwork.Size} {artwork.Description} {artwork.Prompt} ");
            // Convert artworks to CollectibleDto and call LoadGltfAssetsAndAddComponents
        }
        meshColliderAndXRGrabAdder.LoadGltfAssetsAndAddComponents(artworks);
    }
}

public class ArtworkRegistryService
{
    private Web3 web3;
    private Contract contract;

    public ArtworkRegistryService(string rpcUrl, string contractAddress)
    {
        web3 = new Web3(rpcUrl);
        string abi = @"[{
            ""constant"": true,
            ""inputs"": [],
            ""name"": ""getArtworkCount"",
            ""outputs"": [{""name"": """", ""type"": ""uint256""}],
            ""payable"": false,
            ""stateMutability"": ""view"",
            ""type"": ""function""
        },{
            ""constant"": true,
            ""inputs"": [{""name"": ""_index"", ""type"": ""uint256""}],
            ""name"": ""getArtwork"",
            ""outputs"": [
                {""name"": ""name"", ""type"": ""string""},
                {""name"": ""description"", ""type"": ""string""},
                {""name"": ""prompt"", ""type"": ""string""},
                {""name"": ""fileUrl"", ""type"": ""string""},
                {""name"": ""x"", ""type"": ""int256""},
                {""name"": ""y"", ""type"": ""int256""},
                {""name"": ""z"", ""type"": ""int256""},
                {""name"": ""size"", ""type"": ""uint256""}
            ],
            ""payable"": false,
            ""stateMutability"": ""view"",
            ""type"": ""function""
        }]";
        contract = web3.Eth.GetContract(abi, contractAddress);
    }

    public Task<BigInteger> GetArtworkCountAsync()
    {
        var getArtworkCountFunction = contract.GetFunction("getArtworkCount");
        return getArtworkCountFunction.CallAsync<BigInteger>();
    }

    public Task<ArtworkDTO> GetArtworkAsync(BigInteger index)
    {
        var getArtworkFunction = contract.GetFunction("getArtwork");
        return getArtworkFunction.CallDeserializingToObjectAsync<ArtworkDTO>(index);
    }

    [FunctionOutput]
    public class ArtworkDTO : IFunctionOutputDTO
    {
        [Parameter("string", "name", 1)] public string Name { get; set; }
        [Parameter("string", "description", 2)] public string Description { get; set; }
        [Parameter("string", "prompt", 3)] public string Prompt { get; set; }
        [Parameter("string", "fileUrl", 4)] public string FileUrl { get; set; }
        [Parameter("int256", "x", 5)] public BigInteger X { get; set; }
        [Parameter("int256", "y", 6)] public BigInteger Y { get; set; }
        [Parameter("int256", "z", 7)] public BigInteger Z { get; set; }
        [Parameter("uint256", "size", 8)] public BigInteger Size { get; set; }
    }

}
