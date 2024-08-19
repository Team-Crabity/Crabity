using UnityEngine;
using Steamworks;

public class LeaderboardManager : MonoBehaviour
{
    private SteamLeaderboard_t leaderboardHandle;
    private CallResult<LeaderboardFindResult_t> findLeaderboardResult;
    private CallResult<LeaderboardScoresDownloaded_t> downloadScoresResult;

private void Start()
{
    if (!SteamManager.Initialized)
    {
        Debug.LogError("Steam is not initialized.");
        return;
    }

    // Create the CallResults for asynchronous operations
    findLeaderboardResult = CallResult<LeaderboardFindResult_t>.Create(OnLeaderboardFindResult);
    downloadScoresResult = CallResult<LeaderboardScoresDownloaded_t>.Create(OnLeaderboardScoresDownloaded);

    // Find the leaderboard at the start of the game
    FindLeaderboard("Daily Time Trial");
}

    private void FindLeaderboard(string leaderboardName)
    {
        // Request to find the leaderboard on Steam
        SteamAPICall_t handle = SteamUserStats.FindLeaderboard(leaderboardName);
        findLeaderboardResult.Set(handle);
    }

    private void OnLeaderboardFindResult(LeaderboardFindResult_t pCallback, bool bIOFailure)
    {
        if (!bIOFailure && pCallback.m_bLeaderboardFound != 0)
        {
            leaderboardHandle = pCallback.m_hSteamLeaderboard;
            Debug.Log("Leaderboard found: " + leaderboardHandle);
        }
        else
        {
            Debug.LogError("Failed to find leaderboard.");
        }
    }

    public void UploadScore(int score)
    {
        if (leaderboardHandle.m_SteamLeaderboard != 0)
        {
            // Upload the player's score to the leaderboard
            SteamUserStats.UploadLeaderboardScore(leaderboardHandle, ELeaderboardUploadScoreMethod.k_ELeaderboardUploadScoreMethodKeepBest, score, null, 0);
            Debug.Log("Score uploaded: " + score);
        }
        else
        {
            Debug.LogError("Leaderboard handle is invalid, cannot upload score.");
        }
    }

    public void GetLeaderboardEntries()
    {
        if (leaderboardHandle.m_SteamLeaderboard != 0)
        {
            // Request to download the top 10 leaderboard entries
            SteamAPICall_t handle = SteamUserStats.DownloadLeaderboardEntries(leaderboardHandle, ELeaderboardDataRequest.k_ELeaderboardDataRequestGlobal, 0, 10);
            downloadScoresResult.Set(handle);
        }
        else
        {
            Debug.LogError("Leaderboard handle is invalid, cannot get leaderboard entries.");
        }
    }

    private void OnLeaderboardScoresDownloaded(LeaderboardScoresDownloaded_t pCallback, bool bIOFailure)
    {
        if (!bIOFailure && pCallback.m_cEntryCount > 0)
        {
            Debug.Log("Leaderboard entries downloaded: " + pCallback.m_cEntryCount);

            for (int i = 0; i < pCallback.m_cEntryCount; i++)
            {
                LeaderboardEntry_t entry;
                SteamUserStats.GetDownloadedLeaderboardEntry(pCallback.m_hSteamLeaderboardEntries, i, out entry, null, 0);
                Debug.Log($"Player: {SteamFriends.GetFriendPersonaName(entry.m_steamIDUser)}, Score: {entry.m_nScore}");
            }
        }
        else
        {
            Debug.LogError("Failed to download leaderboard entries or no entries found.");
        }
    }

    private void Update()
    {
        // Process Steam callbacks
        if (SteamManager.Initialized)
        {
            SteamAPI.RunCallbacks();
        }
    }
}
