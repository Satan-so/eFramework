<%@ Control Language="C#" AutoEventWireup="true" Inherits="GHY.EF.Admin.controls.pager" %>
<div class="pager">
    <div class="pager-nav">
        <a class="start-page" href="javascript:void;">首页</a>
        <a class="pre-page" href="javascript:void;">上一页</a>
        <a class="next-page" href="javascript:void;">下一页</a>
        <a class="end-page" href="javascript:void;">末页</a>
    </div>

    <div class="pager-count">
        共
        <span class="info-count"></span>
        项
        当前
        <span class="page-count"></span>
        页
    </div>
</div>
<script type="text/javascript" src="/js/pager.js"></script>