import React from "react";
import {
  BrowserRouter,
  Redirect,
  Switch,
} from "react-router-dom";
import Navigation from "./components/NavBar";
import { getAsRoleRoutes } from "./Routes";

const MainRouter = () => (
  <BrowserRouter>
    <>
      <Navigation />
      <div className="content-wrapper">
        <Switch>
          {getAsRoleRoutes()}
          <Redirect path="*" to="/" />
        </Switch>
      </div>
    </>
  </BrowserRouter>
);

export default MainRouter;
