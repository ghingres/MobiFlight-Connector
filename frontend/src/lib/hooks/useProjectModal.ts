import { useLocation, useNavigate } from "react-router"

export function useProjectModal() {
  const navigate = useNavigate()
  const location = useLocation()

  const showOverlay = () => {
    navigate("/project/new", { state: { backgroundLocation: location } })
  }

  const showStandalone = () => {
    navigate("/project/new")
  }

  return { showOverlay, showStandalone }
}