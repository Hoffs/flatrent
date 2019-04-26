import React, { Component, ReactNode } from "react";
import InfiniteScroll from "react-infinite-scroller";
import { RouteComponentProps } from "react-router-dom";
import { toast } from "react-toastify";
import FlexRow from "../../components/FlexRow";
import FlatService from "../../services/FlatService";
import { IFlatListItem, IFlatListResponse } from "../../services/interfaces/FlatServiceInterfaces";
import FlatBox, { FlatBoxLoader } from "./FlatBox";
import Styles from "./FlatList.module.css";
import { IApiResponse } from "../../services/interfaces/Common";

class FlatList extends Component<
  RouteComponentProps,
  { pageSize: number; page: number; hasMore: boolean; flats: IFlatListItem[] }
> {
  constructor(props: Readonly<RouteComponentProps>) {
    super(props);
    this.state = { pageSize: 20, page: 0, hasMore: true, flats: [] };
    this.loadFlats(this.state.page);
  }

  public render() {
    return (
      <FlexRow className={Styles.flatBoxes}>
        {/* <FlatFilters onPageCountChange={console.log} onShowRentedChange={this.handleShowRentedChange} /> */}
        <InfiniteScroll
          className={Styles.scroller}
          pageStart={0}
          initialLoad={false}
          loadMore={this.loadFlats}
          hasMore={this.state.hasMore}
          loader={this.getLoaderItems(4)}
          useWindow={true}
        >
          {this.getFlatItems()}
        </InfiniteScroll>
      </FlexRow>
    );
  }

  private getFlatItems(): ReactNode[] {
    const flats = this.state.flats.map((flat) => <FlatBox key={flat.id} flat={flat} />);
    if (flats.length > 0) {
      return flats;
    } else {
      return this.getLoaderItems(12);
    }
  }

  private getLoaderItems(count: number): ReactNode[] {
    return Array(count).fill(0).map((_, idx) => <FlatBoxLoader key={idx} />);
  }

  private loadFlats = (pageNumber: number) => {
    FlatService.getFlats(this.state.pageSize, this.state.pageSize * pageNumber)
      .then(this.handleFlatResult)
      .catch(this.handleFail);
  }

  private handleFlatResult = (result: IApiResponse<IFlatListItem[]>) => {
    if (result.errors !== undefined) {
      const errors = Object.keys(result.errors).map((key) => result.errors![key].join("\n"));
      errors.forEach((error) => toast.error(error));
    } else if (result.data !== undefined) {
      this.setState((state) => ({ flats: [...state.flats, ...result.data!], hasMore: result.data!.length !== 0 }));
    }
  }

  private handleFail(e: any) {
    toast.error("Įvyko nežinoma klaida.");
  }
}

export default FlatList;
