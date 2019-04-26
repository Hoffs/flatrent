import { createBrowserHistory } from "history";
import React from "react";
import {
  BrowserRouter,
  Redirect,
  Router,
  Switch,
} from "react-router-dom";
import Footer from "./components/Footer";
import Navigation from "./components/NavBar";
import { getAsRoleRoutes } from "./Routes";

export const history = createBrowserHistory();

const MainRouter = () => (
  <Router history={history} >
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
  </Router>
);

export default MainRouter;
