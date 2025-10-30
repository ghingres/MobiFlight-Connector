import { ProjectSummary } from "@/types/project"
import {
  IconDeviceGamepad2,
  IconDotsVertical,
  IconFile,
  IconPlaneDeparture,
  IconPlayerPlayFilled,
  IconPlayerStopFilled,
  IconStar,
  IconStarFilled,
} from "@tabler/icons-react"
import { Badge } from "@/components/ui/badge"
import { HtmlHTMLAttributes } from "react"
import { cn } from "@/lib/utils"
import { Button } from "../ui/button"
import TwoStateIcon from "../icons/TwoStateIcon"
import { useTranslation } from "react-i18next"

export type ProjectCardProps = HtmlHTMLAttributes<HTMLDivElement> & {
  summary: ProjectSummary
}

const ProjectCard = ({
  summary,
  className,
  ...otherProps
}: ProjectCardProps) => {
  const { t } = useTranslation()
  const isRunning = summary.Name === "Fenix A320"
  const isAvailable = summary.Sims.every((sim) => sim.Available)
  const imageUrl =
    summary.Thumbnail ||
    `/aircraft/default/${summary.Sims[0].Name.toLowerCase()}.jpg`

  return (
    <div
      {...otherProps}
      className={cn(
        className,
        "border-border bg-card hover:bg-accent space-y-2 rounded-md p-4 shadow-md transition-all duration-200 ease-in-out hover:shadow-lg",
      )}
    >
      <div className="flex flex-row items-center justify-between">
        <div className="flex flex-row items-center justify-start gap-2">
          {summary.Favorite ? (
            <IconStarFilled className="h-6 fill-amber-400 stroke-amber-400" />
          ) : (
            <IconStar className="stroke-muted-foreground h-6" />
          )}
          <h2 className="truncate text-xl font-medium">{summary.Name}</h2>
        </div>
        <div>
          <IconDotsVertical className="text-muted-foreground h-6" />
        </div>
      </div>
      <div className="bg-accent h-48 w-full grow rounded-md">
        <img
          src={new URL(imageUrl, import.meta.url).href}
          alt={summary.Name}
          className="h-full w-full rounded-md object-cover"
        />
      </div>
      <div className="text-muted-foreground flex flex-row items-center justify-items-center gap-2">
        {summary.Sims.map((s) => {
          const bgColor = s.Available ? "bg-primary" : "bg-muted-foreground"
          return (
            <Badge key={s.Name} className={bgColor}>
              {s.Name}
            </Badge>
          )
        })}
      </div>
      <div className="flex flex-row gap-2">
        {summary.Aircraft[0] && (
          <IconPlaneDeparture
            className={
              summary.Aircraft[0].Available
                ? "text-primary"
                : "text-muted-foreground"
            }
          />
        )}
        <p className="text-muted-foreground truncate">
          {summary.Aircraft.map((a) => `${a.Name} (${a.Filter})`).join(", ")}
        </p>
      </div>
      <div className="flex flex-row items-center justify-between">
        <div className="flex flex-row items-center justify-items-center gap-4">
          <div className="flex flex-row gap-2">
            <IconDeviceGamepad2 className="text-muted-foreground" />
            <p className="text-muted-foreground">
              {summary.Controllers.length}
            </p>
          </div>
          <div className="flex flex-row gap-2">
            <IconFile className="text-muted-foreground" />
            <p className="text-muted-foreground">5</p>
          </div>
        </div>
        <Button
          // disabled={isTesting}
          variant="ghost"
          className="text-md h-8 gap-1 p-1 [&_svg]:size-6"
          onClick={() =>
            // handleMenuItemClick({ action: !isRunning ? "run" : "stop" })
            console.log("Run/Stop clicked")
          }
        >
          <TwoStateIcon
            state={isRunning}
            primaryIcon={IconPlayerPlayFilled}
            secondaryIcon={IconPlayerStopFilled}
            primaryClassName={
              isAvailable
                ? "fill-green-600 stroke-green-600"
                : "fill-none stroke-2 stroke-muted-foreground"
            }
            secondaryClassName="fill-red-700 stroke-red-700"
          />
          <div className="hidden pr-1 lg:inline-flex">
            {!isRunning
              ? t("Project.Execution.Run.Label")
              : t("Project.Execution.Run.Stop")}
          </div>
        </Button>
      </div>
      <div className="flex flex-row justify-end">
        <div className="inline-flex"></div>
      </div>
    </div>
  )
}

export default ProjectCard
