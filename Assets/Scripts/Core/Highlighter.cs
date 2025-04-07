using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace Core
{
    public class Highlighter: MonoBehaviour
    {
        private bool _highlighted;
        
        [SerializeField] private Light2D light;
        [SerializeField] private float highlightTimeInSeconds = 0.4f;
        [SerializeField] private float tickTimeInSeconds = 0.05f;
        [SerializeField] private ParticleSystem particles;

        private void Start()
        {
            light = GetComponent<Light2D>();
        }

        public void Highlight()
        {
            StartCoroutine(HighlightUpgradeCoroutine());
            particles.Play();
        }
        
        private IEnumerator HighlightUpgradeCoroutine()
        {
            if (_highlighted) yield break;
            _highlighted = true;
            var tickCount = highlightTimeInSeconds/tickTimeInSeconds;
            for (var i = 0; i < tickCount/2; i++)
            {
                light.intensity += 1;
                yield return new WaitForSeconds(tickTimeInSeconds);
            }
            for (var i = 0; i < tickCount/2; i++)
            {
                light.intensity -= 1;
                yield return new WaitForSeconds(tickTimeInSeconds);
            }

            light.intensity = 0;
            _highlighted = false;
        }
    }
}