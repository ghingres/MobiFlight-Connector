import { ProjectSummary } from "@/types/project"
import { HtmlHTMLAttributes } from "react"
import { Badge } from "../ui/badge"
import { cn } from "@/lib/utils"
import { Button } from "../ui/button"

export type ProjectListItemProps = HtmlHTMLAttributes<HTMLDivElement> & {
  summary: ProjectSummary
}

const ProjectListItem = ({
  summary: project,
  className,
  ...props
}: ProjectListItemProps) => {
  return (
    <div className={cn(
      "group flex flex-row gap-2 p-2 hover:bg-accent items-center justify-between",
      className)} {...props}>
      <h3 className="font-semibold w-1/4">{project.Name}</h3>
      <p className="w-1/4 flex flex-row gap-1">
        {project.Sims.map((s) => {
          const bgColor = s.Available ? "bg-primary" : "bg-muted-foreground"
          return (
            <Badge key={s.Name} className={bgColor}>
              {s.Name}
            </Badge>
          )
        })}
      </p>
      <div className="w-24">
      <Button className="hidden group-hover:inline-flex h-8">Open</Button>
      </div>
    </div>
  )
}

export default ProjectListItem
