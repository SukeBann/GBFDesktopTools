using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace GBFDesktopTools.View
{
    public class AsyncCollection<T> : ObservableCollection<T>
    {
        System.Windows.Threading.Dispatcher Dispatcher = System.Windows.Threading.Dispatcher.CurrentDispatcher;

        public AsyncCollection() { }

        public AsyncCollection(IEnumerable<T> list) : base(list) { }

        public new void Add(T item)
        {
            if (Dispatcher.Thread == System.Threading.Thread.CurrentThread)
            {
                base.Add(item);
            }
            else
            {
                Dispatcher.Invoke(new Action(() =>
                {
                    base.Add(item);
                }), System.Windows.Threading.DispatcherPriority.Normal);
            }
        }

        /// <summary>
        /// 从 System.Collections.ObjectModel.Collection<T> 中移除所有元素。
        /// </summary>
        public new void Clear()
        {
            if (Dispatcher.Thread == System.Threading.Thread.CurrentThread)
            {
                base.Clear();
            }
            else
            {
                Dispatcher.Invoke(new Action(() =>
                {
                    base.Clear();
                }), System.Windows.Threading.DispatcherPriority.Normal);
            }
        }

        /// <summary>
        /// null：add；true：continue；false：break
        /// </summary>
        /// <param name="_List"></param>
        /// <param name="doEach">null：add；true：continue；false：break</param>
        public void AddRange(IEnumerable<T> _List, System.Func<T, bool?> doEach = null)
        {
            if (_List != null)
            {
                lock (this)
                {
                    foreach (var x in _List)
                    {
                        if (doEach != null)
                        {
                            var r = doEach(x);
                            if (null == r)
                            {
                                this.Add(x);
                            }
                            else if (true == r) { }
                            else
                            {
                                break;
                            }
                        }
                        else
                        {
                            this.Add(x);
                        }
                    }
                }
            }
        }

        public new void Remove(T item)
        {
            if (Dispatcher.Thread == System.Threading.Thread.CurrentThread)
            {
                base.Remove(item);
            }
            else
            {
                Dispatcher.Invoke(new Action(() =>
                {
                    base.Remove(item);
                }), System.Windows.Threading.DispatcherPriority.Normal);
            }
        }

        public new void Insert(int index, T item)
        {
            if (Dispatcher.Thread == System.Threading.Thread.CurrentThread)
            {
                base.Insert(index, item);
            }
            else
            {
                Dispatcher.Invoke(new Action(() =>
                {
                    base.Insert(index, item);
                }), System.Windows.Threading.DispatcherPriority.Normal);
            }
        }

        // ReSharper disable once UnusedMember.Global
        public void RemoveAt(int index)
        {
            if (Dispatcher.Thread == System.Threading.Thread.CurrentThread)
            {
                base.RemoveAt(index);
            }
            else
            {
                Dispatcher.Invoke(new Action(() =>
                {
                    base.RemoveAt(index);
                }), System.Windows.Threading.DispatcherPriority.Normal);
            }
        }
    }
}
