// import ControllerMainCard from "@/components/controllers/ControllerMainCard"
import CommunityMainCard from "@/components/community/CommunityMainCard"
import ProjectMainCard from "@/components/project/ProjectMainCard"

const Dashboard = () => {
  return (
    <div className="grid grid-cols-1 gap-2 overflow-hidden border-none xl:grid-cols-3">
      <div className="xl:col-span-2">
        <ProjectMainCard />
      </div>
      <div className="row-span-2 hidden xl:block">
        <CommunityMainCard />
      </div>
      {/* <div className="xl:col-span-2 2xl:col-span-2">
        <ControllerMainCard />
      </div> */}
    </div>
  )
}

export default Dashboard
