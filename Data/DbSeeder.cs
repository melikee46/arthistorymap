using Microsoft.EntityFrameworkCore;
using ArtHistoryMap.Api.Entities;

namespace ArtHistoryMap.Api.Data
{
    public static class DbSeeder
    {
        public static async Task SeedAsync(AppDbContext context)
        {
            // Idempotent: veri varsa hiçbir şey yapma
            if (await context.ArtMovements.AnyAsync()) return;

            // ---------- Yardımcı yerel fonksiyonlar ----------
            ArtMovement M(string name, int start, int end, string region, string description) => new()
            {
                Id = Guid.NewGuid(),
                Name = name,
                StartYear = start,
                EndYear = end,
                Region = region,
                Description = description
            };

            // Okuma kuralı: Source <type> Target. Source = SONRA gelen akım.
            ArtMovementRelation Rel(ArtMovement source, RelationType type, ArtMovement target, string description) => new()
            {
                Id = Guid.NewGuid(),
                SourceMovementId = source.Id,
                SourceMovement = source,
                TargetMovementId = target.Id,
                TargetMovement = target,
                RelationType = type,
                Description = description
            };

            Artist A(string name, int? birth, int? death, string nationality) => new()
            {
                Id = Guid.NewGuid(),
                Name = name,
                BirthYear = birth,
                DeathYear = death,
                Nationality = nationality
            };

            ArtistMovement Link(Artist artist, ArtMovement movement) => new()
            {
                ArtistId = artist.Id,
                Artist = artist,
                MovementId = movement.Id,
                Movement = movement
            };

            // ---------- Akımlar ----------
            var ronesans = M("Rönesans", 1400, 1600, "İtalya",
                "Antik çağın yeniden keşfi, perspektif ve insan merkezli dünya görüşü.");
            var barok = M("Barok", 1600, 1750, "İtalya",
                "Dramatik ışık-gölge, hareket ve duygusal yoğunluk.");
            var rokoko = M("Rokoko", 1730, 1780, "Fransa",
                "Barok'un hafif, süslü ve oyunbaz aristokrat versiyonu.");
            var neoklasisizm = M("Neoklasisizm", 1760, 1830, "Fransa",
                "Antik Yunan-Roma ideallerine dönüş; düzen, akıl ve sadelik.");
            var romantizm = M("Romantizm", 1800, 1850, "Fransa",
                "Duygu, hayal gücü, doğanın yüceliği ve bireysel deneyim.");
            var akademik = M("Akademik Sanat", 1648, 1880, "Fransa",
                "Sanat akademilerinin belirlediği kurallara dayalı resmî sanat anlayışı.");
            var empresyonizm = M("Empresyonizm", 1860, 1886, "Fransa",
                "Işığın ve anlık izlenimin açık havada yakalanması.");
            var kubizm = M("Kübizm", 1907, 1922, "Fransa",
                "Nesnelerin geometrik formlara ayrılıp birden çok açıdan gösterilmesi.");
            var surrealizm = M("Sürrealizm", 1924, 1966, "Fransa",
                "Bilinçdışı, rüya ve mantık dışı imgelerin sanatı.");

            var movements = new[]
            {
                ronesans, barok, rokoko, neoklasisizm, romantizm,
                akademik, empresyonizm, kubizm, surrealizm
            };

            // ---------- İlişkiler ----------
            var relations = new List<ArtMovementRelation>
            {
                Rel(barok, RelationType.InfluencedBy, ronesans,
                    "Barok, Rönesans'ın perspektif ve anatomi mirası üzerine kuruldu."),
                Rel(barok, RelationType.ReactionTo, ronesans,
                    "Rönesans'ın dengeli sakinliğine karşı dramatik hareket ve gerilim."),

                Rel(rokoko, RelationType.InfluencedBy, barok,
                    "Süsleme ve eğri formları Barok'tan devraldı."),
                Rel(rokoko, RelationType.ReactionTo, barok,
                    "Barok'un ağırlığına karşı hafif, pastel ve oyunbaz bir dil."),
                Rel(rokoko, RelationType.ContemporaryWith, barok,
                    "1730-1750 arasında iki akım birlikte var oldu."),

                Rel(neoklasisizm, RelationType.ReactionTo, rokoko,
                    "Rokoko'nun gösterişli hafifliğine karşı ahlaki ciddiyet ve sadelik."),
                Rel(neoklasisizm, RelationType.InfluencedBy, ronesans,
                    "Antik ideallerin Rönesans'taki yeniden keşfinden beslendi."),

                Rel(romantizm, RelationType.ReactionTo, neoklasisizm,
                    "Neoklasik akla ve düzene karşı duygu ve hayal gücü."),
                Rel(romantizm, RelationType.ContemporaryWith, neoklasisizm,
                    "1800-1830 arasında iki akım yan yana, çatışarak yaşadı."),

                Rel(akademik, RelationType.InfluencedBy, neoklasisizm,
                    "Akademilerin kural ve çizim disiplini Neoklasisizm'e dayanır."),

                Rel(empresyonizm, RelationType.ReactionTo, akademik,
                    "Akademinin atölye ve konu hiyerarşisine karşı açık hava ve anlık izlenim."),
                Rel(empresyonizm, RelationType.InfluencedBy, romantizm,
                    "Romantizm'in renk ve atmosfer vurgusunu sürdürdü."),

                Rel(kubizm, RelationType.InfluencedBy, empresyonizm,
                    "Cézanne üzerinden form ve yapı arayışına evrildi."),

                Rel(surrealizm, RelationType.InfluencedBy, romantizm,
                    "Hayal gücü ve rüya vurgusunun modern devamı."),
                Rel(surrealizm, RelationType.InfluencedBy, kubizm,
                    "Gerçekliği parçalayıp yeniden kurma cesaretini Kübizm'den aldı.")
            };

            // ---------- Sanatçılar ----------
            var leonardo = A("Leonardo da Vinci", 1452, 1519, "İtalyan");
            var michelangelo = A("Michelangelo", 1475, 1564, "İtalyan");
            var caravaggio = A("Caravaggio", 1571, 1610, "İtalyan");
            var rembrandt = A("Rembrandt", 1606, 1669, "Hollandalı");
            var watteau = A("Antoine Watteau", 1684, 1721, "Fransız");
            var boucher = A("François Boucher", 1703, 1770, "Fransız");
            var david = A("Jacques-Louis David", 1748, 1825, "Fransız");
            var ingres = A("Jean-Auguste-Dominique Ingres", 1780, 1867, "Fransız");
            var delacroix = A("Eugène Delacroix", 1798, 1863, "Fransız");
            var friedrich = A("Caspar David Friedrich", 1774, 1840, "Alman");
            var cabanel = A("Alexandre Cabanel", 1823, 1889, "Fransız");
            var monet = A("Claude Monet", 1840, 1926, "Fransız");
            var renoir = A("Pierre-Auguste Renoir", 1841, 1919, "Fransız");
            var picasso = A("Pablo Picasso", 1881, 1973, "İspanyol");
            var braque = A("Georges Braque", 1882, 1963, "Fransız");
            var dali = A("Salvador Dalí", 1904, 1989, "İspanyol");
            var magritte = A("René Magritte", 1898, 1967, "Belçikalı");

            var artists = new[]
            {
                leonardo, michelangelo, caravaggio, rembrandt, watteau, boucher,
                david, ingres, delacroix, friedrich, cabanel, monet, renoir,
                picasso, braque, dali, magritte
            };

            // ---------- Sanatçı-Akım bağlantıları (many-to-many) ----------
            var links = new List<ArtistMovement>
            {
                Link(leonardo, ronesans), Link(michelangelo, ronesans),
                Link(caravaggio, barok), Link(rembrandt, barok),
                Link(watteau, rokoko), Link(boucher, rokoko),
                Link(david, neoklasisizm),
                Link(ingres, neoklasisizm), Link(ingres, akademik), // bir sanatçı birden fazla akıma ait
                Link(delacroix, romantizm), Link(friedrich, romantizm),
                Link(cabanel, akademik),
                Link(monet, empresyonizm), Link(renoir, empresyonizm),
                Link(picasso, kubizm), Link(braque, kubizm),
                Link(dali, surrealizm), Link(magritte, surrealizm)
            };

            context.ArtMovements.AddRange(movements);
            context.Artists.AddRange(artists);
            context.ArtMovementRelations.AddRange(relations);
            context.ArtistMovements.AddRange(links);

            // Tek SaveChanges = tek transaction
            await context.SaveChangesAsync();
        }
    }
}