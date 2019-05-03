import React, { ComponentClass, FunctionComponent } from "react";
import RoleRoute from "./components/RoleRoute";
import AgreementDetails from "./scenes/Agreement";
import FlatList from "./scenes/Flats";
import CreateFlat from "./scenes/Flats/scenes/FlatCreate";
import FlatDetails from "./scenes/Flats/scenes/FlatDetails";
import Login from "./scenes/Login";
import Logout from "./scenes/Logout";
import Profile from "./scenes/Profile";
import Register from "./scenes/Register";
import UserService, { Policies } from "./services/UserService";

export enum Authentication {
  Anonymous = 0,
  Authenticated = 1,
  Either = 2,
}

interface IRouteInfo {
  order: number;
  addToNav: boolean;
  navOnly?: boolean;
  authentication: Authentication;
  exact?: boolean;
  link: string;
  redirect: string;
  text?: string;
  roles: number[];
  component: ComponentClass<any> | FunctionComponent<any>;
}

// 10
const AuthRoutes: IRouteInfo[] = [
  {
    addToNav: true,
    authentication: Authentication.Anonymous,
    component: Login,
    link: "/login",
    order: 10,
    redirect: "/",
    roles: [],
    text: "Prisijungti",
  },
  {
    addToNav: true,
    authentication: Authentication.Anonymous,
    component: Register,
    link: "/register",
    order: 11,
    redirect: "/",
    roles: [],
    text: "Registruotis",
  },
  {
    addToNav: true,
    authentication: Authentication.Authenticated,
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
    addToNav: false,
    authentication: Authentication.Either,
    component: FlatList,
    exact: true,
    link: "/",
    order: 50,
    redirect: "/login",
    roles: [],
    text: "Butai",
  },
  {
    addToNav: true,
    authentication: Authentication.Either,
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
    authentication: Authentication.Authenticated,
    component: CreateFlat,
    exact: true,
    link: "/flats/create",
    order: 51,
    redirect: "/",
    roles: Policies.User,
    text: "Išnuomoti butą",
  },
  {
    addToNav: false,
    authentication: Authentication.Either,
    component: FlatDetails,
    link: "/flat/:id",
    order: 100,
    redirect: "/login",
    roles: [],
    text: "Butas",
  },
  {
    addToNav: false,
    authentication: Authentication.Authenticated,
    component: CreateFlat,
    link: "/flat/:id/edit",
    order: 100,
    redirect: "/login",
    roles: [],
    text: "Butas",
  },
];

// 60
const AgreementRoutes: IRouteInfo[] = [
  {
    addToNav: false,
    authentication: Authentication.Authenticated,
    component: AgreementDetails,
    link: "/agreement/:id",
    order: 60,
    redirect: "/",
    roles: [],
  },
];

// 90
const UserRoutes: IRouteInfo[] = [
  {
    addToNav: true,
    authentication: Authentication.Authenticated,
    component: Profile,
    link: `/user/${UserService.userId()}`,
    navOnly: true,
    order: 90,
    redirect: "/",
    roles: [],
    text: "Paskyra",
  },
  {
    addToNav: false,
    authentication: Authentication.Authenticated,
    component: Profile,
    link: "/user/:id",
    order: 90,
    redirect: "/",
    roles: [],
  },
];

export const sortByOrder = (a: IRouteInfo, b: IRouteInfo): number => a.order - b.order;
export const filterApplicable = (route: IRouteInfo): boolean =>
  UserService.satisfiesAuthentication(route.authentication) && UserService.hasRoles(...route.roles);

export const Routes: IRouteInfo[] = [
  ...AuthRoutes,
  // {
  //   addToNav: true,
  //   authenticated: true,
  //   component: () => <Redirect to="/flats" />,
  //   exact: true,
  //   link: "/",
  //   order: 1,
  //   redirect: "/login",
  //   roles: [],
  //   text: "Pradžia",
  // },
  ...FlatRoutes,
  ...AgreementRoutes,
  ...UserRoutes,
].sort(sortByOrder);

export const getAsRoleRoutes = () => {
  return Routes.filter((route) => route.navOnly !== true)
    .sort(sortByOrder)
    .map((link, index) => (
      <RoleRoute
        key={index}
        path={link.link}
        exact={link.exact}
        redirect={link.redirect}
        authenticated={link.authentication}
        allowedRoles={link.roles}
        component={link.component}
      />
    ));
};

export const getApplicableRoutes = () => Routes.filter(filterApplicable);
