import IconBrandMobiFlightLogo from "@/components/icons/IconBrandMobiFlightLogo"
import { Button } from "@/components/ui/button"
import {
  Card,
  CardContent,
  CardDescription,
  CardHeader,
  CardTitle,
} from "@/components/ui/card"
import useMessageExchange from "@/lib/hooks/useMessageExchange"
import { cn } from "@/lib/utils"
import Markdown, { Components } from "react-markdown"

interface CommunityPost {
  title: string
  tags: string[]
  date: string
  content: string[]
  featured?: boolean
  action?: {
    title: string
    url: string
  }
  media?: {
    type: "image" | "video"
    src: string
    alt: string
  }
}

const CommunityMainCard = () => {
  const { publish } = useMessageExchange()

  const communityFeed = [
    {
      title: "100% Dedication to MobiFlight!",
      date: "2025-11-30",
      content:[
        "This year, I decided to commit myself full-time to MobiFlight development.",
        "Please consider supporting us through donations to help sustain the continuous development and growth of MobiFlight.",
        ],
      featured: false,
      tags: ["Community"],
      media: {
        type: "image",
        src: "/feed/full-time.jpg",
        alt: "100% dedication to Flight Simulation",
      },
      action: {
        title: "Donate to MobiFlight",
        url: "https://mobiflight.com/donate",
      },
    },
    {
      title: "New products in the MobiFlight Store",
      date: "2025-11-30",
      content:[
        "Check out the latest additions to our store, including new modules and accessories to enhance your flight simulation experience.",
      ],
      featured: true,
      tags: ["Shop"],
      media: {
        type: "image",
        src: "/feed/shop-new-boards.jpg",
        alt: "New products in MobiFlight Store",
      },
      action: {
        title: "Register Now",
        url: "https://shop.mobiflight.com",
      },
    },
    {
      title: "Subscribe to Our Newsletter",
      date: "2025-11-30",
      content:[
        "Stay updated with the **latest news, tutorials, and community highlights** by subscribing to [our newsletter](https://mobiflight.com/newsletter).",
      ],
      action: {
        title: "Subscribe",
        url: "https://mobiflight.com/newsletter",
      },
      media: {
        type: "image",
        src: "/feed/newsletter-subscribe.jpg",
        alt: "Subscribe to Our Newsletter",
      },
      tags: ["Community"],
    },
  ] as CommunityPost[]

  const handleLinkClick = (e: React.MouseEvent<HTMLAnchorElement>) => {
    e.preventDefault()
    const url = (e.target as HTMLAnchorElement).href
    openUrl(url)
  }

  const openUrl = (url: string) => {
    publish({
      key: "CommandOpenLinkInBrowser",
      payload: { url: url },
    })
  }

  const renderLink: Components["a"] = (props) => {
    return (
      <a
        {...props}
        onClick={handleLinkClick}
        className="text-blue-600 underline"
      />
    )
  }

  return (
    <Card className="border-shadow-none bg-muted h-full rounded-none">
      <CardHeader>
        <CardTitle className="flex flex-row gap-2">
          <IconBrandMobiFlightLogo /> Community Feed
        </CardTitle>
        <CardDescription>
          News and updates from the MobiFlight community.
        </CardDescription>
      </CardHeader>
      <CardContent>
        <div className="flex flex-col gap-4">
          {communityFeed.map((post) => (
            <div
              key={post.title}
              className={cn(
                "border-muted 4xl:flex-row flex flex-row justify-between gap-8 border-b p-8 lg:flex-col",
                post.featured && "bg-background rounded-md",
              )}
            >
              {post.media && post.media.type === "image" && (
                <div className="w-1/2 lg:w-full max-h-48">
                  <img
                    className="w-full rounded-lg object-cover max-h-48"
                    src={post.media.src}
                    alt={post.media.alt}
                  />
                </div>
              )}
              <div className="flex w-1/2 flex-col justify-between gap-4 lg:w-full">
                <div>
                  <h4 className="text-xl font-semibold">{post.title}</h4>
                  <div className="text-muted-foreground">{post.date}</div>
                  <div className="text-sm flex flex-col gap-4">
                    {post.content.map((paragraph, index) => (
                      <Markdown key={index} components={{ a: renderLink }}>
                        {paragraph}
                      </Markdown>
                    ))}
                  </div>
                </div>
                {post.action && (
                  <div className="">
                    <Button
                      size={"sm"}
                      onClick={() => openUrl(post.action!.url)}
                    >
                      {post.action!.title}
                    </Button>
                  </div>
                )}
              </div>
            </div>
          ))}
        </div>
      </CardContent>
    </Card>
  )
}

export default CommunityMainCard
