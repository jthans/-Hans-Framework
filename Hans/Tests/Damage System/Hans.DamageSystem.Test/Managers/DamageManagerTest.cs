using System.Collections.Generic;
using Hans.DamageSystem.Interfaces;
using Hans.DamageSystem.Models;
using System.Linq;
using System.ComponentModel.Composition;

namespace Hans.DamageSystem.Test.Managers
{
    /// <summary>
    ///  Test manager to test damage controllers by directly tracking layers and other classes.
    /// </summary>
    [Export(typeof(IDamageDataManager))]
    public class DamageManagerTest : IDamageDataManager
    {
        decimal entityHealth;
        List<LayerMask> layerMasks = new List<LayerMask>();

        public void AddLayer(string entityId, LayerMask layerMask)
        {
            this.layerMasks.Add(layerMask);
        }

        public Dictionary<string, decimal> ApplyDamage(string entityId, Dictionary<string, decimal> damageToApply)
        {
            this.entityHealth -= damageToApply["BaseHealth"];
            return new Dictionary<string, decimal>() { { "BaseHealth", this.entityHealth } };
        }

        public void BeginTrackingDamage(string entityId, decimal startHealth)
        {
            this.entityHealth = startHealth;
        }

        public void EndTrackingDamage(string entityId)
        {
            throw new System.NotImplementedException();
        }

        public LayerMask[] GetLayersForEntity(string entityId)
        {
            return this.layerMasks.ToArray();
        }

        public void RemoveLayer(string entityId, string layerId)
        {
            this.layerMasks.Remove(this.layerMasks.FirstOrDefault(x => x.Id == layerId));
        }

        public void UpdateLayer(string entityId, LayerMask layerMask)
        {
            this.RemoveLayer(null, layerMask.Id);
            this.AddLayer(null, layerMask);
        }
    }
}
