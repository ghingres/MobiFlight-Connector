import CommunityFeedFilter from "@/components/community/CommunityFeedFilter"
import CommunityFeedItem from "@/components/community/CommunityFeedItem"
import IconBrandMobiFlightLogo from "@/components/icons/IconBrandMobiFlightLogo"

import {
  Card,
  CardContent,
  CardDescription,
  CardHeader,
  CardTitle,
} from "@/components/ui/card"
import { CommunityPost } from "@/types/feed"
import { useSearchParams } from "react-router"



const CommunityMainCard = () => {
  const [searchParams] = useSearchParams()
  const activeFilter = searchParams.get("feed_filter") || "all"

  const communityFeed = [
    {
      title: "100% Dedication to MobiFlight!",
      date: "2025-11-30",
      content: [
        "This year, I decided to commit myself full-time to MobiFlight development.",
        "Please consider supporting us through donations to help sustain the continuous development and growth of MobiFlight.",
      ],
      featured: false,
      tags: ["community"],
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
      content: [
        "Check out the latest additions to our store, including new modules and accessories to enhance your flight simulation experience.",
      ],
      featured: true,
      tags: ["shop"],
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
      content: [
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
      tags: ["community"],
    },
    {
      title: "Meet us in Paderborn",
      date: "2025-10-30",
      content: [
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
      tags: ["events"],
    },
    {
      title: "Subscribe to Our Newsletter",
      date: "2025-11-30",
      content: [
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
      tags: ["community"],
    },
    {
      title: "Subscribe to Our Newsletter",
      date: "2025-11-30",
      content: [
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
      tags: ["community"],
    },
  ] as CommunityPost[]

  return (
    <Card className="border-shadow-none bg-muted flex h-full flex-col rounded-none">
      <CardHeader>
        <CardTitle className="flex flex-row gap-2">
          <IconBrandMobiFlightLogo /> Community Feed
        </CardTitle>
        <CardDescription>
          News and updates from the MobiFlight community.
        </CardDescription>
      </CardHeader>
      <CardContent className="flex h-full flex-col gap-2">
        <CommunityFeedFilter />
        <div className="relative grow p-4">
          <div className="absolute inset-0 overflow-auto">
            {communityFeed.map((post, index) => (
              post.tags.includes(activeFilter) || activeFilter === "all" ? (
                <CommunityFeedItem key={index} post={post} />
              ) : null 
            ))}
          </div>
        </div>
      </CardContent>
    </Card>
  )
}

export default CommunityMainCard
