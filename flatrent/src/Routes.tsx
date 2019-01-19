import React, { ComponentClass, FunctionComponent } from "react";
import RoleRoute from "./components/RoleRoute";
import CreateFlat from "./scenes/FlatCreate";
import FlatList from "./scenes/Flats";
import Login from "./scenes/Login";
import Logout from "./scenes/Logout";
import { Policies } from "./services/UserService";
import Register from "./scenes/Register";
import { number } from "prop-types";
import FlatDetails from "./scenes/FlatDetails";
import Profile from "./scenes/Profile";
import { Redirect } from "react-router-dom";

interface IRouteInfo {
  order: number;
  addToNav: boolean;
  authenticated: boolean;
  exact?: boolean;
  link: string;
  redirect: string;
  text: string;
  roles: string[];
  component: ComponentClass<any> | FunctionComponent<any>;
}

// 10
const AuthRoutes: IRouteInfo[] = [
  {
    addToNav: true,
    authenticated: false,
    component: Login,
    link: "/login",
    order: 10,
    redirect: "/",
    roles: [],
    text: "Prisijungti",
  },
  {
    addToNav: true,
    authenticated: false,
    component: Register,
    link: "/register",
    order: 11,
    redirect: "/",
    roles: [],
    text: "Registruotis",
  },
  {
    addToNav: true,
    authenticated: true,
    component: Logout,
    link: "/logout",
    order: 200,
    redirect: "/login",
    roles: [],
    text: "Atsijungti",
  },
];

// 50+
const FlatRoutes: IRouteInfo[] = [
  {
    addToNav: true,
    authenticated: true,
    component: FlatList,
    exact: true,
    link: "/flats",
    order: 50,
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
    order: 51,
    redirect: "/",
    roles: Policies.Supply,
    text: "Naujas butas",
  },
  {
    addToNav: false,
    authenticated: true,
    component: FlatDetails,
    link: "/flat/:id",
    order: 100,
    redirect: "/login",
    roles: [],
    text: "Butas",
  },
];

// 90
const UserRoutes: IRouteInfo[] = [
  {
    addToNav: true,
    authenticated: true,
    component: Profile,
    link: "/user",
    order: 90,
    redirect: "/",
    roles: Policies.Client,
    text: "Paskyra",
  },
];

export const Routes: IRouteInfo[] = [
  ...AuthRoutes,
  {
    addToNav: true,
    authenticated: true,
    component: () => <Redirect to="/flats" />,
    exact: true,
    link: "/",
    order: 1,
    redirect: "/login",
    roles: [],
    text: "Pradžia",
  },
  ...FlatRoutes,
  ...UserRoutes,
];

export const getAsRoleRoutes = () => {
  return Routes.sort(sortByOrder).map((link, index) => (
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

const sortByOrder = (a: IRouteInfo, b: IRouteInfo): number => a.order - b.order;
