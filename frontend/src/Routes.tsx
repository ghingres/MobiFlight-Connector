import App from "@/App";
import ConfigListPage from "@/pages/ConfigList";
import Dashboard from "@/pages/Dashboard";
import { BrowserRouter, Route, Routes } from "react-router";

export function AppRoutes() {
  return (
    <BrowserRouter>
        <Routes>
          <Route path="/home" element={<App />} >
            <Route index element={<Dashboard />} />
          </Route>
          <Route path="/config" element={<App />}>
            <Route index element={<ConfigListPage />} />
          </Route>
          <Route index path="/index.html" element={<App />} />
        </Routes>
      </BrowserRouter>
  )
}