using System.Collections.Generic;
using System.Configuration;
using System.Reflection;

namespace FFXI.XIACE {

    //- int off = (int)OFFSET.EQUIP_INFO + ((int)slot * 8) + 4;
    //+ int off = (int) Offset.Get("EQUIP_INFO") + ((int) slot * 8) + 4;

    public class Offset {

        private static Dictionary<string, Offset> dict = new Dictionary<string, Offset>();
        private int offset;

        static Offset() {
            load();
        }

        public Offset(int offset) {
            this.offset = offset;
        }

        public static explicit operator Offset(int value) {
            return new Offset(value);
        }

        public static explicit operator int(Offset obj) {
            return obj.offset;
        }

        public static int operator +(int value, Offset obj) {
            return value + obj.offset;
        }

        public static Dictionary<string, Offset> Get() {
            return dict;
        }

        public static Offset Get(string name) {
            return (dict.ContainsKey(name)) ? dict[name] : (Offset) 0;
        }

        public static void load() {

            Configuration config = ConfigurationManager.OpenExeConfiguration(Assembly.GetExecutingAssembly().Location);
            OffsetSection section = (OffsetSection) config.GetSection("OffsetSection");

            OffsetElementCollection collection = section.OffsetElementCollection;

            foreach (OffsetElement element in collection) {
                if (dict.ContainsKey(element.name)) {
                    dict[element.name].offset = element.value;
                } else {
                    dict.Add(element.name, new Offset(element.value));
                }
            }
        }
    }

    public class OffsetSection : ConfigurationSection {

        [ConfigurationProperty("OffsetCollection")]
        public OffsetElementCollection OffsetElementCollection {
            get { return (OffsetElementCollection) this["OffsetCollection"]; }
        }
    }

    public class OffsetElement : ConfigurationElement {

        [ConfigurationProperty("name", IsRequired = true)]
        public string name {
            get { return (string) this["name"]; }
        }

        [ConfigurationProperty("value", DefaultValue = 0)]
        public int value {
            get { return (int) this["value"]; }
        }
    }

    public class OffsetElementCollection : ConfigurationElementCollection {

        public OffsetElementCollection() {
            this.AddElementName = "Offset";
        }

        public OffsetElement Get(int index) {
            return (OffsetElement) base.BaseGet(index);
        }

        public OffsetElement Get(string name) {
            return (OffsetElement) BaseGet(name);
        }

        public bool Contains(string name) {
            return BaseGet(name) != null;
        }

        protected override ConfigurationElement CreateNewElement() {
            return new OffsetElement();
        }

        protected override object GetElementKey(ConfigurationElement element) {
            return ((OffsetElement) element).name;
        }
    }
}
