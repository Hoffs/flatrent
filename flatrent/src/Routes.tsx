import React, { ComponentClass, FunctionComponent } from "react";
import RoleRoute from "./components/RoleRoute";
import CreateFlat from "./scenes/FlatCreate";
import FlatList from "./scenes/Flats";
import Login from "./scenes/Login";
import Logout from "./scenes/Logout";
import { Policies } from "./services/UserService";
import Register from "./scenes/Register";

interface IRouteInfo {
  addToNav: boolean;
  authenticated: boolean;
  exact?: boolean;
  link: string;
  redirect: string;
  text: string;
  roles: string[];
  component: ComponentClass<any> | FunctionComponent<any>;
}

export const Routes: IRouteInfo[] = [
  {
    addToNav: true,
    authenticated: false,
    component: Login,
    link: "/login",
    redirect: "/",
    roles: [],
    text: "Prisijungti",
  },
  {
    addToNav: true,
    authenticated: false,
    component: Register,
    link: "/register",
    redirect: "/",
    roles: [],
    text: "Registruotis",
  },
  {
    addToNav: true,
    authenticated: true,
    component: Login,
    exact: true,
    link: "/",
    redirect: "/login",
    roles: [],
    text: "Pradžia",
  },
  {
    addToNav: true,
    authenticated: true,
    component: FlatList,
    exact: true,
    link: "/flats",
    redirect: "/login",
    roles: [],
    text: "Butai",
  },
  {
    addToNav: true,
    authenticated: true,
    component: CreateFlat,
    exact: true,
    link: "/flats/create",
    redirect: "/",
    roles: Policies.Supply,
    text: "Naujas butas",
  },
  {
    addToNav: false,
    authenticated: true,
    component: Login,
    link: "/flat/:id",
    redirect: "/login",
    roles: [],
    text: "Butas",
  },
  {
    addToNav: true,
    authenticated: true,
    component: Login,
    link: "/user",
    redirect: "/",
    roles: Policies.Client,
    text: "Paskyra",
  },
  {
    addToNav: true,
    authenticated: true,
    component: Logout,
    link: "/logout",
    redirect: "/login",
    roles: [],
    text: "Atsijungti",
  },
];

export const getAsRoleRoutes = () => {
  return Routes.map((link, index) => (
    <RoleRoute
      key={index}
      path={link.link}
      exact={link.exact}
      redirect={link.redirect}
      authenticated={link.authenticated}
      allowedRoles={link.roles}
      component={link.component}
    />
  ));
};
