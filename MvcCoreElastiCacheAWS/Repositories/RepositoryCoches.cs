using MvcCoreElastiCacheAWS.Models;
using System.Xml.Linq;

namespace MvcCoreElastiCacheAWS.Repositories
{
    public class RepositoryCoches
    {
        private XDocument document;

        public RepositoryCoches()
        {
            string path = "MvcCoreElastiCacheAWS.Documents.coches.xml";
            Stream stream = this.GetType().Assembly.GetManifestResourceStream(path)!;
            this.document = XDocument.Load(stream);
        }

        public List<Coche>? GetCoches()
        {
            List<Coche>? consulta = this.document.Descendants("coche")
                .Select(e => new Coche
                {
                    IdCoche = int.Parse(e.Element("idcoche")!.Value),
                    Marca = e.Element("marca")!.Value,
                    Modelo = e.Element("modelo")!.Value,
                    Imagen = e.Element("imagen")!.Value
                }).ToList();
            return consulta;
        }

        public Coche? GetCoche(int id)
        {
            return
                this.GetCoches().FirstOrDefault(coche => coche.IdCoche.Equals(id));
        }
    }
}
