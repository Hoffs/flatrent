import React from "react";
import {
  BrowserRouter,
  Redirect,
  Switch,
} from "react-router-dom";
import Nav from "./components/NavBar";
import { getAsRoleRoutes } from "./Routes";

const AppRouter = () => (
  <BrowserRouter>
    <>
      <Nav />
      <div className="content-wrapper">
        <Switch>
          {getAsRoleRoutes()}
          <Redirect path="*" to="/" />
        </Switch>
      </div>
    </>
  </BrowserRouter>
);

export default AppRouter;
