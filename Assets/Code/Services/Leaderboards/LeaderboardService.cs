﻿using System;
using System.Collections.Generic;
using System.Linq;
using Code.Services.GoogleSpreadsheet;
using UnityEngine;

namespace Code.Services.Leaderboards
{
    public class LeaderboardService : MonoBehaviour
    {
        [SerializeField] private string _spreadSheetId;
        [SerializeField] private string _workSheetName;
        [SerializeField] private string _password;
        [SerializeField] private int _topSize = 10;

        private SpreadSheetRetriever<LeaderboardEntry[]> _spreadSheetGet;
        private SpreadSheetRetriever<string> _spreadSheetSet;

        public LeaderboardPosition[] Top;
        public LeaderboardPosition[] Surroundings;

        public Action LeaderboardUpdated;
        
        private LeaderboardEntry[] _leaderboard;
        private string _alias = "";

        private void Awake()
        {
            _spreadSheetGet = new SpreadSheetRetriever<LeaderboardEntry[]>(this);
            _spreadSheetSet = new SpreadSheetRetriever<string>(this);
        }

        private void Start()
        {
            _spreadSheetGet.ResponseSuccessful += OnGetResponseSuccessful;
            _spreadSheetGet.ResponseFailed += OnGetResponseFailed;
            _spreadSheetSet.ResponseSuccessful += OnSetResponseSuccessful;
            _spreadSheetSet.ResponseFailed += OnSetResponseFailed;
            GetData();
        }
        
        private void GetData()
        {
            _spreadSheetGet.GetData(_spreadSheetId, _workSheetName, _password);
        }

        public void SetAlias(string alias)
        {
            _alias = alias;
        }

        public void NewEntry(int score)
        {
            _spreadSheetSet.SetData(_spreadSheetId, _workSheetName,new []{_alias, score.ToString()},  _password);
        }

        private void OnGetResponseSuccessful(LeaderboardEntry[] leaderboard)
        {
            int topAmount = Mathf.Min(_topSize, leaderboard.Length);
            var top = leaderboard.Take(topAmount).ToArray();
            Top = new LeaderboardPosition[top.Length];
            
            for (var i = 0; i < top.Length; i++)
            {
                Top[i] = new LeaderboardPosition(i + 1, top[i]);
            }

            _leaderboard = leaderboard;
            GetSurroundings();
            LeaderboardUpdated?.Invoke();
        }

        private void GetSurroundings()
        {
            if (string.IsNullOrEmpty(_alias))
            {
                Surroundings = null;
                return;
            }
            
            int topAmount = Mathf.Min(_topSize, _leaderboard.Length);
            LeaderboardEntry firstEntry = _leaderboard.FirstOrDefault(x => x.Alias == _alias);

            if (firstEntry == null)
            {
                Surroundings = null;
                return;
            }

            int halfTopAmount = topAmount % 2 == 0 ? (topAmount / 2) - 1 : topAmount / 2; 
            int index = Array.IndexOf(_leaderboard, firstEntry);
            int bottomSize = _leaderboard.Length - index - 1;
            int maxIndex = bottomSize > halfTopAmount ? halfTopAmount : bottomSize;
            int minIndex = index > halfTopAmount ? topAmount - maxIndex - 1: index;

            if (minIndex <= halfTopAmount)
            {
                maxIndex = topAmount - minIndex - 1;
            }
            
            Surroundings = new LeaderboardPosition[topAmount];

            for (int i = index - minIndex; i <= index + maxIndex; i++)
            {
                Surroundings[i - (index - minIndex)] = new LeaderboardPosition(i + 1, _leaderboard[i]);
            }
            
            Debug.Log("Hola");
        }

        public LeaderboardPosition[] GetPossibleSurroundings(int score)
        {
            LeaderboardEntry fakeEntry = new LeaderboardEntry()
            {
                Alias = "",
                Date = DateTime.UtcNow.ToString(),
                Score = score
            };
            List<LeaderboardEntry> leaderboard = _leaderboard.ToList();

            int index = -1;
            for (int i = 0; i < _leaderboard.Length; i++)
            {
                if (_leaderboard[i].Score < score)
                {
                    index = i;
                    leaderboard.Insert(index, fakeEntry);
                    break;
                }
            }

            if (index == -1)
            {
                leaderboard.Add(fakeEntry);
                index = leaderboard.Count - 1;
            }

            int topAmount = Mathf.Min(7, leaderboard.Count);

            int halfTopAmount = topAmount % 2 == 0 ? (topAmount / 2) - 1 : topAmount / 2;
            int bottomSize = leaderboard.Count - index - 1;
            int maxIndex = bottomSize > halfTopAmount ? halfTopAmount : bottomSize;
            int minIndex = index > halfTopAmount ? topAmount - maxIndex - 1: index;

            if (minIndex <= halfTopAmount)
            {
                maxIndex = topAmount - minIndex - 1;
            }
            
            var result = new LeaderboardPosition[topAmount];

            for (int i = index - minIndex; i <= index + maxIndex; i++)
            {
                result[i - (index - minIndex)] = new LeaderboardPosition(i + 1, leaderboard[i]);

                if (index == i)
                {
                    result[i - (index - minIndex)].IsFake = true;
                }
            }

            return result;
        }

        private void OnGetResponseFailed()
        {
            
        }

        private void OnSetResponseSuccessful(string obj)
        {
            GetData();
        }

        private void OnSetResponseFailed()
        {
            
        }

        public string GetAlias()
        {
            return _alias;
        }
    }
}