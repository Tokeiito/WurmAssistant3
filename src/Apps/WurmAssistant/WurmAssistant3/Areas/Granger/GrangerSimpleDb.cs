﻿using System.Collections.Generic;
using AldursLab.PersistentObjects;
using AldursLab.WurmAssistant3.Areas.Granger.DataLayer;
using Newtonsoft.Json;

namespace AldursLab.WurmAssistant3.Areas.Granger
{
    /// <summary>
    /// This class is an adapted DAL from when GrangerDb was using SqLite.
    /// </summary>
    [KernelBind(BindingHint.Singleton), PersistentObject("GrangerFeature_GrangerSimpleDb")]
    public class GrangerSimpleDb : PersistentObjectBase
    {
        public GrangerSimpleDb()
        {
            Creatures = new Dictionary<int, CreatureEntity>();
            Herds = new Dictionary<string, HerdEntity>();
            TraitValues = new Dictionary<int, TraitValueEntity>();
            CreatureColors = new Dictionary<string, CreatureColorEntity>();
        }

        [JsonProperty("creatures")]
        public Dictionary<int, CreatureEntity> Creatures { get; private set; }
        [JsonProperty("herds")]
        public Dictionary<string, HerdEntity> Herds { get; private set; }
        [JsonProperty("traitValues")]
        public Dictionary<int, TraitValueEntity> TraitValues { get; private set; }
        [JsonProperty("creatureColors")]
        public Dictionary<string, CreatureColorEntity> CreatureColors { get; private set; }

        public void Save()
        {
            RequestSave(forceSave:true);
        } 
    }
}
