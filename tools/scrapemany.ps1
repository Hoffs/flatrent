$urls = (
    "https://www.aruodas.lt/butu-nuoma-klaipedoje-paupiuose-klemiskes-g-butas-labai-siltas-ir-sviesus-nes-langai-i-4-897009/",
    "https://www.aruodas.lt/butu-nuoma-vilniuje-snipiskese-sporto-g-nuomojamas-irengtas-4-k-butas-paskutiniame-4-897277/",
    "https://www.aruodas.lt/butu-nuoma-vilniuje-snipiskese-rinktines-g-isnuomojamas-irengtas-2-kambariu-butas-su-4-897269/",
    
)

foreach ($url in $urls) {
    py ./aruodasripper.py $url
}