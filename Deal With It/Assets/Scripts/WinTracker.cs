using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WinTracker : MonoBehaviour
{
    // NPC info
    private NPC npc;

    // Played cards info
    private PlayedActionCardsDisplay playedCards;

    // Source for wins
    private RoundController roundStats;

    // Tracker objects: Duration + Range
    [SerializeField] private Toggle durationToggle;
    [SerializeField] private TMP_Text durationText;
    [SerializeField] private TMP_Text durationCounter;

    [SerializeField] private Toggle joyRangeToggle;
    [SerializeField] private TMP_Text joyRangeText;

    [SerializeField] private Toggle sadnessRangeToggle;
    [SerializeField] private TMP_Text sadnessRangeText;

    [SerializeField] private Toggle fearRangeToggle;
    [SerializeField] private TMP_Text fearRangeText;

    [SerializeField] private Toggle angerRangeToggle;
    [SerializeField] private TMP_Text angerRangeText;

    // Tracker objects: Max Strategy per Round
    [SerializeField] private Toggle maxDistractionToggle;
    [SerializeField] private TMP_Text maxDistractionText;
    [SerializeField] private TMP_Text maxDistractionCounter;

    [SerializeField] private Toggle maxExpressionToggle;
    [SerializeField] private TMP_Text maxExpressionText;
    [SerializeField] private TMP_Text maxExpressionCounter;

    [SerializeField] private Toggle maxProcessingToggle;
    [SerializeField] private TMP_Text maxProcessingText;
    [SerializeField] private TMP_Text maxProcessingCounter;

    [SerializeField] private Toggle maxReappraisalToggle;
    [SerializeField] private TMP_Text maxReappraisalText;
    [SerializeField] private TMP_Text maxReappraisalCounter;

    // Tracker objects: Total Strategy in Game
    [SerializeField] private Toggle minDistractionToggle;
    [SerializeField] private TMP_Text minDistractionText;
    [SerializeField] private TMP_Text minDistractionCounter;

    [SerializeField] private Toggle minExpressionToggle;
    [SerializeField] private TMP_Text minExpressionText;
    [SerializeField] private TMP_Text minExpressionCounter;

    [SerializeField] private Toggle minProcessingToggle;
    [SerializeField] private TMP_Text minProcessingText;
    [SerializeField] private TMP_Text minProcessingCounter;

    [SerializeField] private Toggle minReappraisalToggle;
    [SerializeField] private TMP_Text minReappraisalText;
    [SerializeField] private TMP_Text minReappraisalCounter;

    // Start is called before the first frame update
    void Start()
    {
        // Initialize NPC
        NPCDisplay NPCDisplay = (NPCDisplay)GameObject.FindGameObjectWithTag("NPC").GetComponent(typeof(NPCDisplay));
        npc = NPCDisplay.npc;

        // Initialize PlayedActionCards
        playedCards = (PlayedActionCardsDisplay)GameObject.FindGameObjectWithTag("Played Action Cards Display").GetComponent(typeof(PlayedActionCardsDisplay));

        // Initialize the RoundController
        roundStats = (RoundController)GameObject.FindGameObjectWithTag("Round Controller").GetComponent(typeof(RoundController));

        // Set up tracker
        SetUpConditions();
        ShowStrategyConditions();
    }

    // Update is called once per frame
    void Update()
    {
        // Counters
        UpdateCounters();

        // Toggles
        TrackConditions();
    }

    // Apply NPC conditions to tracker
    private void SetUpConditions()
    {
        // Duration
        durationText.text = npc.RangeWinDuration.ToString();

        // Ranges
        joyRangeText.text = npc.JoyRange.x.ToString() + "-" + npc.JoyRange.y.ToString();
        sadnessRangeText.text = npc.SadnessRange.x.ToString() + "-" + npc.SadnessRange.y.ToString();
        fearRangeText.text = npc.FearRange.x.ToString() + "-" + npc.FearRange.y.ToString();
        angerRangeText.text = npc.AngerRange.x.ToString() + "-" + npc.AngerRange.y.ToString();

        // Max Strategy per Round
        maxDistractionText.text = npc.MaxDistractionPerRound.ToString();
        maxExpressionText.text = npc.MaxExpressionPerRound.ToString();
        maxProcessingText.text = npc.MaxProcessingPerRound.ToString();
        maxReappraisalText.text = npc.MaxReappraisalPerRound.ToString();

        // Total Strategy in Game
        minDistractionText.text = npc.MinDistractionTotal.ToString();
        minExpressionText.text = npc.MinExpressionTotal.ToString();
        minProcessingText.text = npc.MinProcessingTotal.ToString();
        minReappraisalText.text = npc.MinReappraisalTotal.ToString();
    }

    // Show applicable conditions
    private void ShowStrategyConditions()
    {
        // Max Strategy per Round
        maxDistractionToggle.gameObject.SetActive(npc.MaxDistractionPerRound < 5);
        maxExpressionToggle.gameObject.SetActive(npc.MaxExpressionPerRound < 5);
        maxProcessingToggle.gameObject.SetActive(npc.MaxProcessingPerRound < 5);
        maxReappraisalToggle.gameObject.SetActive(npc.MaxReappraisalPerRound < 5);

        // Total Strategy in Game
        minDistractionToggle.gameObject.SetActive(npc.MinDistractionTotal > 0);
        minExpressionToggle.gameObject.SetActive(npc.MinExpressionTotal > 0);
        minProcessingToggle.gameObject.SetActive(npc.MinProcessingTotal > 0);
        minReappraisalToggle.gameObject.SetActive(npc.MinReappraisalTotal > 0);
    }

    // Update condition counters
    private void UpdateCounters()
    {
        // Duration
        durationCounter.text = FormatCounter(roundStats.GoalCount, npc.RangeWinDuration);

        // Max Strategy per Round
        maxDistractionCounter.text = FormatCounter(playedCards.DistractionCount, npc.MaxDistractionPerRound);
        maxExpressionCounter.text = FormatCounter(playedCards.ExpressionCount, npc.MaxExpressionPerRound);
        maxProcessingCounter.text = FormatCounter(playedCards.ProcessingCount, npc.MaxProcessingPerRound);
        maxReappraisalCounter.text = FormatCounter(playedCards.ReappraisalCount, npc.MaxReappraisalPerRound);

        // Total Strategy in Game
        minDistractionCounter.text = FormatCounter(roundStats.TotalDistractionCount, npc.MinDistractionTotal);
        minExpressionCounter.text = FormatCounter(roundStats.TotalExpressionCount, npc.MinExpressionTotal);
        minProcessingCounter.text = FormatCounter(roundStats.TotalProcessingCount, npc.MinProcessingTotal);
        minReappraisalCounter.text = FormatCounter(roundStats.TotalReappraisalCount, npc.MinReappraisalTotal);
    }

    // Format counter i.e. "(N/X)"
    private string FormatCounter(int count, int total)
    {
        return "(" + count.ToString() + "/" + total.ToString() + ")";
    }

    // Update toggles
    private void TrackConditions()
    {
        // Duration
        durationToggle.isOn = roundStats.DurationWinStatus;

        // Ranges
        joyRangeToggle.isOn = CheckRange(npc.JoyLvl, npc.JoyRange.x, npc.JoyRange.y);
        sadnessRangeToggle.isOn = CheckRange(npc.SadnessLvl, npc.SadnessRange.x, npc.SadnessRange.y);
        fearRangeToggle.isOn = CheckRange(npc.FearLvl, npc.FearRange.x, npc.FearRange.y);
        angerRangeToggle.isOn = CheckRange(npc.AngerLvl, npc.AngerRange.x, npc.AngerRange.y);

        // Max Strategy per Round
        maxDistractionToggle.isOn = (playedCards.DistractionCount < npc.MaxDistractionPerRound);
        maxExpressionToggle.isOn = (playedCards.ExpressionCount < npc.MaxExpressionPerRound);
        maxProcessingToggle.isOn = (playedCards.ProcessingCount < npc.MaxProcessingPerRound);
        maxReappraisalToggle.isOn = (playedCards.ReappraisalCount < npc.MaxReappraisalPerRound);

        // Total Strategy in Game
        minDistractionToggle.isOn = (npc.MinDistractionTotal <= roundStats.TotalDistractionCount);
        minExpressionToggle.isOn = (npc.MinExpressionTotal <= roundStats.TotalExpressionCount);
        minProcessingToggle.isOn = (npc.MinProcessingTotal <= roundStats.TotalProcessingCount);
        minReappraisalToggle.isOn = (npc.MinReappraisalTotal <= roundStats.TotalReappraisalCount);
    }

    private bool CheckRange(int current, float min, float max)
    {
        return min <= current && current <= max;
    }
}
