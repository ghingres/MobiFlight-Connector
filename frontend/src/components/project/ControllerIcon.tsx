import { cn } from "@/lib/utils"
import {
  IconDeviceGamepad2,
  IconPiano,
  IconQuestionMark,
} from "@tabler/icons-react"
import { HtmlHTMLAttributes } from "react"

export type ControllerIconProps = {
  serial: string
}

const ControllerIcon = ({
  serial,
  className,
  ...props
}: HtmlHTMLAttributes<HTMLDivElement> & ControllerIconProps) => {
  console.log("ControllerIcon serial:", serial)
  const controllerType = serial.includes("SN-")
    ? "mobiflight"
    : serial.includes("JS-")
      ? "joystick"
      : serial.includes("MI-")
        ? "midi"
        : "unknown"

  const controllerIconUrl = "/controller/type/" + controllerType + ".png"

  return (
    <div
      className={cn(`h-10 w-10 overflow-hidden rounded-full bg-primary border-2 border-background items-center flex justify-center`, className)}
      {...props}
    >
      {controllerType == "mobiflight" && (
        <img
          src={controllerIconUrl}
          alt={`${controllerType} controller icon`}
        />
      )}

      {controllerType == "joystick" && (
        <IconDeviceGamepad2 className="text-background h-8 w-8" />
      )}

      {controllerType == "midi" && (
        <IconPiano className="text-background h-8 w-8" />
      )}

      {controllerType == "unknown" && (
        <IconQuestionMark className="text-background h-8 w-8" />
      )}
    </div>
  )
}

export default ControllerIcon
