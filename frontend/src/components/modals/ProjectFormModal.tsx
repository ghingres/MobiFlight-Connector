import { useNavigate } from "react-router-dom"
import type { Project } from "@/types/project"
import ProjectForm from "@/components/project/ProjectForm"

export default function NewProjectModalRoute() {
  const navigate = useNavigate()
  const close = () => navigate(-1)

  const emptyProject = { Name: "" } as Project

  return (
    <ProjectForm
      project={emptyProject}
      isOpen
      onOpenChange={(open: boolean) => {
        if (!open) close()
      }}
      onSave={async (values) => {
        console.log("Creating project:", values)
        // TODO: await createProject(values)
        close()
      }}
    />
  )
}