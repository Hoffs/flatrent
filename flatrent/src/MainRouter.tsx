import React from "react";
import {
  BrowserRouter,
  Redirect,
  Switch,
} from "react-router-dom";
import Navigation from "./components/NavBar";
import { getAsRoleRoutes } from "./Routes";
import Footer from "./components/Footer";

const MainRouter = () => (
  <BrowserRouter>
    <div className="layout">
      <Navigation />
      <div className="content-wrapper">
        <Switch>
          {getAsRoleRoutes()}
          <Redirect path="*" to="/" />
        </Switch>
      </div>
      <Footer />
    </div>
  </BrowserRouter>
);

export default MainRouter;
