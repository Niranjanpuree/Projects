import * as React from 'react';
import * as ReactDOM from 'react-dom';
import { toODataString } from '@progress/kendo-data-query';
import { currencyDisplay } from '@telerik/kendo-intl';

interface ILoadingPanelProps { }
interface ILoadingPanelState { }

export class LoadingPanel extends React.Component<ILoadingPanelProps, ILoadingPanelState> {

    render() {
        return (
            <div className="k-loading-mask dataloader">
                <span className="k-loading-text">Loading</span>
                <div className="k-loading-image"></div>
                <div className="k-loading-color"></div>
            </div>
        );

        //const gridContent = document && document.querySelector('.k-grid-content');
        //return gridContent ? ReactDOM.createPortal(loadingPanel, gridContent) : loadingPanel;
        //return loadingPanel;
    }

}