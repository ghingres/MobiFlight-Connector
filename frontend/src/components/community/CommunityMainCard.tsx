import CommunityFeedItem from "@/components/community/CommunityFeedItem"
import IconBrandMobiFlightLogo from "@/components/icons/IconBrandMobiFlightLogo"

import {
  Card,
  CardContent,
  CardDescription,
  CardHeader,
  CardTitle,
} from "@/components/ui/card"

export interface CommunityPost {
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
  const communityFeed = [
    {
      title: "100% Dedication to MobiFlight!",
      date: "2025-11-30",
      content: [
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
      content: [
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
      tags: ["Community"],
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
      tags: ["Community"],
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
      tags: ["Community"],
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
      tags: ["Community"],
    },
  ] as CommunityPost[]

  return (
    <Card className="border-shadow-none bg-muted h-full rounded-none flex flex-col">
      <CardHeader>
        <CardTitle className="flex flex-row gap-2">
          <IconBrandMobiFlightLogo /> Community Feed
        </CardTitle>
        <CardDescription>
          News and updates from the MobiFlight community.
        </CardDescription>
      </CardHeader>
      <CardContent className="h-full p-4 relative">        
        <div className="absolute inset-0 overflow-auto">
          {communityFeed.map((post, index) => (
            <CommunityFeedItem key={index} post={post} />
          ))}          
        </div>
      </CardContent>
    </Card>
  )
}

export default CommunityMainCard
