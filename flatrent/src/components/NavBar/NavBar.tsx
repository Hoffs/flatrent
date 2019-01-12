import React, { Component } from "react";
import { RouteComponentProps, withRouter } from "react-router-dom";
import { Routes } from "../../Routes";
import UserService from "../../services/UserService";
import LinkWithHighlight from "./LinkWithHighlight";
import "./NavBar.css";

class NavBar extends Component<RouteComponentProps> {
  constructor(props: Readonly<RouteComponentProps>) {
    super(props);
  }

  public render() {
    return (
      <div className="top-navbar">
        <nav>
          <ul>
            <li className="top-navbar__item top-navbar__name">Flat Rent Systems</li>
            {this.getLinks()}
          </ul>
        </nav>
      </div>
    );
  }

  private getLinks() {
    const isLoggedIn = UserService.isLoggedIn();
    const filteredLinks = Routes.filter(
      (link) =>
        link.addToNav &&
        (link.authenticated === isLoggedIn || link.authenticated === undefined) &&
        UserService.satisfiesRoles(...link.roles)
    );
    return filteredLinks.map((link, index) => (
      <LinkWithHighlight link={link.link} currentUrl={this.props.location.pathname} key={index}>
        {link.text}
      </LinkWithHighlight>
    ));
  }
}

export default withRouter(NavBar);
