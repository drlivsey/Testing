using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Newtonsoft.Json.Serialization;

using Project.Coords;
using Project.Core;
using Project.JSON;

namespace Project
{
    public class MovingPointsManager : MonoBehaviour
    {
        [SerializeField] private string m_savingPath;
        [SerializeField] private MovingPointsCreator m_pointsCreator;
        [SerializeField] private MovingPointsRegistry m_pointsRegistry;
        [SerializeField] private CoordinatePlane m_coordinateSystem;

        private void OnEnable()
        {
            m_coordinateSystem.OnAxisRepainted += UpdatePointsCoords;
        }

        private void OnDisable()
        {
            m_coordinateSystem.OnAxisRepainted -= UpdatePointsCoords;
        }

        public void ClearPoints()
        {
            m_pointsRegistry.Clear();
        }

        public void SavePoints()
        {
            var points = m_pointsRegistry.GetPoints();
            var pointsInfo = new MovingPointInfo[points.Count()];

            for (var i = 0; i < pointsInfo.Length; i++)
            {
                var point = points.ElementAt(i);
                var pointInfo = new MovingPointInfo();

                pointInfo.Position = new Point(point.Position.x, point.Position.y);
                pointInfo.Coords = point.PointCoords;

                pointsInfo[i] = pointInfo;
            }

            var jsonSaver = new JsonDataManager(m_savingPath);
            jsonSaver.SaveDataInJson(pointsInfo);
        }

        public void LoadPoints()
        {
            var jsonLoader = new JsonDataManager(m_savingPath);
            var points = jsonLoader.LoadDataFromJson<IEnumerable<MovingPointInfo>>();

            m_pointsRegistry.Clear();
            foreach (var pointInfo in points)
            {
                m_pointsCreator.CreateMovingPoint(pointInfo);
            }
        }

        private void UpdatePointsCoords()
        {
            var points = m_pointsRegistry.GetPoints();

            if (points == null) return;

            foreach (var point in points)
            {
                var coords = m_coordinateSystem.PositionToCoords(point.Position);
                point.SetCoords(coords);
            }
        }
    }
}