import { Button } from "@/components/ui/button"
import { useTranslation } from "react-i18next"
import { useSearchParams } from "react-router"

const CommunityFeedFilter = () => {
  const [searchParams, setSearchParams] = useSearchParams()
  const { t } = useTranslation()
  const activeFilter = searchParams.get("feed_filter") || "all"

  const handleFilterChange = (filter: string) => {
    const newParams = new URLSearchParams(searchParams)
    newParams.set("feed_filter", filter)
    setSearchParams(newParams)
  }

  return (
    <div
      className="flex flex-row gap-2"
      data-testid="recent-projects-filter-bar"
    >
      <Button
        className="h-8 px-3 text-sm"
        variant={activeFilter === "all" ? "default" : "outline"}
        onClick={() => handleFilterChange("all")}
      >
        {t("Feed.Filter.all")}
      </Button>
      <Button
        className="h-8 px-3 text-sm"
        variant={activeFilter === "shop" ? "default" : "outline"}
        onClick={() => handleFilterChange("shop")}
      >
        {t("Feed.Filter.shop")}
      </Button>
      <Button
        className="h-8 px-3 text-sm"
        variant={activeFilter === "events" ? "default" : "outline"}
        onClick={() => handleFilterChange("events")}
      >
        {t("Feed.Filter.events")}
      </Button>
    </div>
  )
}

export default CommunityFeedFilter
